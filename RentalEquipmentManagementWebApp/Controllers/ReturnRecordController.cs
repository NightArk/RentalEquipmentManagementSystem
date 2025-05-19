using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.ReturnRecord;
using RentalEquipmentManagementWebApp.Services;


namespace RentalEquipmentManagementWebApp.Controllers
{
    [Authorize(Policy = "RequireAuthenticated")]
    public class ReturnRecordController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EquipmentRentalDBContext _context;
        private readonly IAuditService _auditService;
        private readonly INotificationService _notificationService;


        public ReturnRecordController(UserManager<IdentityUser> userManager, EquipmentRentalDBContext context, IAuditService auditService, INotificationService notificationService)
        {
            _userManager = userManager;
            _context = context;
            _auditService = auditService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
                return Forbid();

            // Determine the user role
            var userRole = await _userManager.GetRolesAsync(identityUser);
            var isAdminOrManager = userRole.Contains("Administrator") || userRole.Contains("Manager");

            ViewData["UserRole"] = isAdminOrManager ? "ManagerOrAdmin" : "Customer";

            List<ReturnRecord> returnRecords;

            if (isAdminOrManager)
            {
                // Admin/Manager: Retrieve all return records
                returnRecords = await _context.ReturnRecords
                    .Include(r => r.RentalTransaction)
                        .ThenInclude(rt => rt.AssignedEquipment)
                    .Include(r => r.RentalTransaction)
                        .ThenInclude(rt => rt.Customer)
                    .ToListAsync();
            }
            else
            {
                // Customer: Retrieve return records for the logged-in user's rental transactions
                var customUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == identityUser.Email);

                if (customUser == null)
                    return Forbid();

                var customerRentalTransactionIds = await _context.RentalTransactions
                    .Where(rt => rt.CustomerId == customUser.Id)
                    .Select(rt => rt.Id)
                    .ToListAsync();

                returnRecords = await _context.ReturnRecords
                    .Include(r => r.RentalTransaction)
                        .ThenInclude(rt => rt.AssignedEquipment)
                    .Include(r => r.RentalTransaction)
                        .ThenInclude(rt => rt.Customer)
                    .Where(r => r.RentalTransactionId.HasValue && customerRentalTransactionIds.Contains(r.RentalTransactionId.Value))
                    .ToListAsync();
            }

            var viewModel = returnRecords.Select(r => new ReturnRecordViewModel
            {
                ReturnId = r.Id,
                RentalTransactionId = r.RentalTransactionId,
                ActualReturnDate = r.ActualReturnDate,
                ReturnCondition = r.ReturnCondition,
                LateReturnFee = r.LateReturnFee,
                AdditionalCharges = r.AdditionalCharges,
                EquipmentId = r.RentalTransaction?.AssignedEquipment?.Id,
                EquipmentName = r.RentalTransaction?.AssignedEquipment?.Name,
                CustomerId = r.RentalTransaction?.CustomerId,
                CustomerName = r.RentalTransaction?.Customer?.Name,
                CustomerEmail = r.RentalTransaction?.Customer?.Email
            }).ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var returnRecord = await _context.ReturnRecords
                .Include(r => r.RentalTransaction)
                    .ThenInclude(rt => rt.AssignedEquipment)
                .Include(r => r.RentalTransaction)
                    .ThenInclude(rt => rt.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (returnRecord == null)
            {
                return NotFound();
            }

            var viewModel = new ReturnRecordViewModel
            {
                ReturnId = returnRecord.Id,
                RentalTransactionId = returnRecord.RentalTransactionId,
                ActualReturnDate = returnRecord.ActualReturnDate,
                ReturnCondition = returnRecord.ReturnCondition,
                LateReturnFee = returnRecord.LateReturnFee,
                AdditionalCharges = returnRecord.AdditionalCharges,
                EquipmentId = returnRecord.RentalTransaction?.AssignedEquipment?.Id,
                EquipmentName = returnRecord.RentalTransaction?.AssignedEquipment?.Name,
                CustomerId = returnRecord.RentalTransaction?.CustomerId,
                CustomerName = returnRecord.RentalTransaction?.Customer?.Name,
                CustomerEmail = returnRecord.RentalTransaction?.Customer?.Email
            };

            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
            {
                return Forbid();
            }

            // Admin or Manager access
            if (await _userManager.IsInRoleAsync(identityUser, "Administrator") || await _userManager.IsInRoleAsync(identityUser, "Manager"))
            {
                return View(viewModel);
            }

            // Customer access - verify ownership
            var customUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == identityUser.Email);
            if (customUser != null && returnRecord.RentalTransaction?.CustomerId == customUser.Id)
            {
                return View(viewModel);
            }

            return Forbid();
        }

        // GET: ReturnRecords/Create
        public IActionResult Create()
        {
            var viewModel = new CreateReturnRecordViewModel
            {
                RentalTransactions = _context.RentalTransactions
                    .Select(rt => new SelectListItem
                    {
                        Value = rt.Id.ToString(),
                        Text = rt.Id.ToString()
                    })
                    .ToList()
            };
            viewModel.RentalTransactions.Insert(0, new SelectListItem { Value = "", Text = "-- Select Transaction --" });
            return View(viewModel);
        }

        // GET: /ReturnRecord/GetTransactionDetails?id=...
        [HttpGet]
        public async Task<IActionResult> GetTransactionDetails(int? id)
        {
            if (id == null)
            {
                return Json(null);
            }

            var transaction = await _context.RentalTransactions
                .Include(rt => rt.AssignedEquipment)
                .Include(rt => rt.Customer)
                .FirstOrDefaultAsync(rt => rt.Id == id);

            if (transaction == null)
            {
                return Json(null);
            }

            return Json(new
            {
                equipmentName = transaction.AssignedEquipment?.Name,
                customerName = transaction.Customer?.Name
            });
        }

        [HttpPost]
        [Authorize(Policy = "RequireManagerRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReturnRecordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var returnRecord = new ReturnRecord
                {
                    RentalTransactionId = viewModel.RentalTransactionId,
                    ActualReturnDate = (DateTime)viewModel.ActualReturnDate,
                    ReturnCondition = viewModel.ReturnCondition,
                    LateReturnFee = viewModel.LateReturnFee,
                    AdditionalCharges = viewModel.AdditionalCharges,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Add(returnRecord);
                await _context.SaveChangesAsync();

                // Get current user
                var userEmail = User.Identity?.Name;
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                // Load rental transaction with customer
                var rentalTransaction = await _context.RentalTransactions
                    .Include(rt => rt.Customer)
                    .FirstOrDefaultAsync(rt => rt.Id == returnRecord.RentalTransactionId);

                if (currentUser != null)
                {
                    // Audit log
                    await _auditService.LogActivityAsync(
                        "Return Record Created",
                        $"Return record ID {returnRecord.Id} was created by {currentUser.Name}.",
                        currentUser.Id
                    );

                    // Notify current user (manager/admin)
                    await _notificationService.CreateNotificationAsync(
                        currentUser.Id,
                        "Return Record Created",
                        $"You created return record ID {returnRecord.Id}."
                    );

                    // Notify customer
                    if (rentalTransaction?.Customer != null)
                    {
                        await _notificationService.CreateNotificationAsync(
                            rentalTransaction.Customer.Id,
                            "New Return Record Created",
                            $"A return record (ID: {returnRecord.Id}) has been created for your rental transaction."
                        );
                    }
                }

                TempData["SuccessMessage"] = $"Return record ID {returnRecord.Id} created successfully.";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate the RentalTransactions dropdown on validation failure
            viewModel.RentalTransactions = _context.RentalTransactions
                .Select(rt => new SelectListItem
                {
                    Value = rt.Id.ToString(),
                    Text = rt.Id.ToString()
                })
                .ToList();
            viewModel.RentalTransactions.Insert(0, new SelectListItem { Value = "", Text = "-- Select Transaction --" });

            return View(viewModel);
        }


        [Authorize(Policy = "RequireManagerRole")]
        // GET: ReturnRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var returnRecord = await _context.ReturnRecords
                .Include(r => r.RentalTransaction)
                    .ThenInclude(rt => rt.AssignedEquipment)
                .Include(r => r.RentalTransaction)
                    .ThenInclude(rt => rt.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (returnRecord == null)
            {
                return NotFound();
            }

            var viewModel = new CreateReturnRecordViewModel
            {
                RentalTransactionId = returnRecord.RentalTransactionId,
                EquipmentName = returnRecord.RentalTransaction?.AssignedEquipment?.Name,
                CustomerName = returnRecord.RentalTransaction?.Customer?.Name,
                ActualReturnDate = returnRecord.ActualReturnDate,
                ReturnCondition = returnRecord.ReturnCondition,
                LateReturnFee = returnRecord.LateReturnFee,
                AdditionalCharges = returnRecord.AdditionalCharges,
                RentalTransactions = _context.RentalTransactions
                    .Select(rt => new SelectListItem
                    {
                        Value = rt.Id.ToString(),
                        Text = rt.Id.ToString()
                    })
                    .ToList()
            };
            viewModel.RentalTransactions.Insert(0, new SelectListItem { Value = "", Text = "-- Select Transaction --" });

            return View(viewModel);
        }

        [Authorize(Policy = "RequireManagerRole")]
        // POST: ReturnRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateReturnRecordViewModel viewModel)
        {
            if (id != _context.ReturnRecords.FindAsync(id).Result?.Id) // Basic check if the record exists
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var returnRecordToUpdate = await _context.ReturnRecords
                        .Include(rr => rr.RentalTransaction)
                            .ThenInclude(rt => rt.Customer)
                        .FirstOrDefaultAsync(rr => rr.Id == id);

                    if (returnRecordToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Update fields
                    returnRecordToUpdate.RentalTransactionId = viewModel.RentalTransactionId;
                    returnRecordToUpdate.ActualReturnDate = (DateTime)viewModel.ActualReturnDate;
                    returnRecordToUpdate.ReturnCondition = viewModel.ReturnCondition;
                    returnRecordToUpdate.LateReturnFee = viewModel.LateReturnFee;
                    returnRecordToUpdate.AdditionalCharges = viewModel.AdditionalCharges;

                    _context.Update(returnRecordToUpdate);
                    await _context.SaveChangesAsync();

                    // Get current user
                    var userEmail = User.Identity?.Name;
                    var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                    if (currentUser != null)
                    {
                        // Audit log
                        await _auditService.LogActivityAsync(
                            "Return Record Edited",
                            $"Return record ID {id} was edited by {currentUser.Name}.",
                            currentUser.Id
                        );

                        // Notify current user (manager/admin)
                        await _notificationService.CreateNotificationAsync(
                            currentUser.Id,
                            "Return Record Edited",
                            $"You edited return record ID {id}."
                        );

                        // Notify customer
                        var customer = returnRecordToUpdate.RentalTransaction?.Customer;
                        if (customer != null)
                        {
                            await _notificationService.CreateNotificationAsync(
                                customer.Id,
                                "Your Return Record Was Updated",
                                $"Your return record ID {id} was updated by {currentUser.Name}."
                            );
                        }
                    }

                    TempData["SuccessMessage"] = $"Return record ID {id} updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReturnRecordExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving.");
                }
            }

            // Repopulate the RentalTransactions dropdown on validation failure
            viewModel.RentalTransactions = _context.RentalTransactions
                .Select(rt => new SelectListItem
                {
                    Value = rt.Id.ToString(),
                    Text = rt.Id.ToString()
                })
                .ToList();
            viewModel.RentalTransactions.Insert(0, new SelectListItem { Value = "", Text = "-- Select Transaction --" });

            return View(viewModel);
        }


        [Authorize(Policy = "RequireManagerRole")]
        // GET: ReturnRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var returnRecord = await _context.ReturnRecords
                .Include(r => r.RentalTransaction)
                    .ThenInclude(rt => rt.AssignedEquipment)
                .Include(r => r.RentalTransaction)
                    .ThenInclude(rt => rt.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (returnRecord == null)
            {
                return NotFound();
            }

            var viewModel = new ReturnRecordViewModel
            {
                ReturnId = returnRecord.Id,
                RentalTransactionId = returnRecord.RentalTransactionId,
                ActualReturnDate = returnRecord.ActualReturnDate,
                ReturnCondition = returnRecord.ReturnCondition,
                LateReturnFee = returnRecord.LateReturnFee,
                AdditionalCharges = returnRecord.AdditionalCharges,
                EquipmentId = returnRecord.RentalTransaction?.AssignedEquipment?.Id,
                EquipmentName = returnRecord.RentalTransaction?.AssignedEquipment?.Name,
                CustomerId = returnRecord.RentalTransaction?.CustomerId,
                CustomerName = returnRecord.RentalTransaction?.Customer?.Name,
                CustomerEmail = returnRecord.RentalTransaction?.Customer?.Email
            };

            return View(viewModel);
        }

        [Authorize(Policy = "RequireManagerRole")]
        // POST: ReturnRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var returnRecord = await _context.ReturnRecords
                .Include(rr => rr.RentalTransaction)
                    .ThenInclude(rt => rt.Customer)
                .FirstOrDefaultAsync(rr => rr.Id == id);

            if (returnRecord == null)
            {
                return NotFound();
            }

            _context.ReturnRecords.Remove(returnRecord);
            await _context.SaveChangesAsync();

            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser != null)
            {
                // Audit log
                await _auditService.LogActivityAsync(
                    "Return Record Deleted",
                    $"Return record ID {id} was deleted by {currentUser.Name}.",
                    currentUser.Id
                );

                // Notify current user (admin/manager)
                await _notificationService.CreateNotificationAsync(
                    currentUser.Id,
                    "Return Record Deleted",
                    $"You deleted return record ID {id}."
                );

                // Notify customer whose record was deleted
                var customer = returnRecord.RentalTransaction?.Customer;
                if (customer != null)
                {
                    await _notificationService.CreateNotificationAsync(
                        customer.Id,
                        "Your Return Record Deleted",
                        $"Your return record ID {id} was deleted by {currentUser.Name}."
                    );
                }
            }

            TempData["SuccessMessage"] = $"Return record ID {id} was deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private bool ReturnRecordExists(int id)
        {
            return _context.ReturnRecords.Any(e => e.Id == id);
        }



    }
}
