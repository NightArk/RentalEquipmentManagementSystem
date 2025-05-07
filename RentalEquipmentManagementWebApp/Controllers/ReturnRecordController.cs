using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentalEquipmentManagementWebApp.Controllers
{
    [Authorize] // All logged-in users can access this controller
    public class ReturnRecordController : Controller
    {
        private readonly EquipmentRentalDBContext _context;

        public ReturnRecordController(EquipmentRentalDBContext context)
        {
            _context = context;
        }

        // GET: ReturnRecords
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Manager"))
            {
                var returnRecords = await _context.ReturnRecords
                    .Include(r => r.RentalTransaction)
                        .ThenInclude(rt => rt.AssignedEquipment)
                    .Include(r => r.RentalTransaction)
                        .ThenInclude(rt => rt.Customer)
                    .ToListAsync();

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
            else if (User.IsInRole("Customer"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var customerRentalTransactions = await _context.RentalTransactions
                        .Where(rt => rt.CustomerId.HasValue && rt.CustomerId.Value.ToString() == userId)
                        .Select(rt => rt.Id)
                        .ToListAsync();

                    var returnRecords = await _context.ReturnRecords
                        .Include(r => r.RentalTransaction)
                            .ThenInclude(rt => rt.AssignedEquipment)
                        .Include(r => r.RentalTransaction)
                            .ThenInclude(rt => rt.Customer)
                        .Where(r => r.RentalTransactionId.HasValue && customerRentalTransactions.Contains(r.RentalTransactionId.Value))
                        .ToListAsync();

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
                else
                {
                    return Forbid();
                }
            }

            return Forbid();
        }

        // GET: ReturnRecords/Details/5
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

            if (User.IsInRole("Admin") || User.IsInRole("Manager") || (User.IsInRole("Customer") && viewModel.CustomerId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier)))
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReturnRecordViewModel viewModel) // Removed [Bind] attribute
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

        [Authorize(Roles = "Manager,Admin")]
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

        [Authorize(Roles = "Manager,Admin")]
        // POST: ReturnRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateReturnRecordViewModel viewModel) // Removed [Bind] attribute
        {
            if (id != _context.ReturnRecords.FindAsync(id).Result?.Id) // Basic check if the record exists
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var returnRecordToUpdate = await _context.ReturnRecords.FindAsync(id);
                    if (returnRecordToUpdate == null)
                    {
                        return NotFound();
                    }

                    returnRecordToUpdate.RentalTransactionId = viewModel.RentalTransactionId;
                    returnRecordToUpdate.ActualReturnDate = (DateTime)viewModel.ActualReturnDate;
                    returnRecordToUpdate.ReturnCondition = viewModel.ReturnCondition;
                    returnRecordToUpdate.LateReturnFee = viewModel.LateReturnFee;
                    returnRecordToUpdate.AdditionalCharges = viewModel.AdditionalCharges;

                    _context.Update(returnRecordToUpdate);
                    await _context.SaveChangesAsync();
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


        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        // POST: ReturnRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var returnRecord = await _context.ReturnRecords.FindAsync(id);
            _context.ReturnRecords.Remove(returnRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReturnRecordExists(int id)
        {
            return _context.ReturnRecords.Any(e => e.Id == id);
        }
    }
}