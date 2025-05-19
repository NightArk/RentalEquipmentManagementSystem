using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.RentalTransaction;
using RentalEquipmentManagementWebApp.Services;

namespace RentalEquipmentManagementWebApp.Controllers
{
    [Authorize(Policy = "RequireAuthenticated")]
    public class RentalTransactionsController : Controller
    {
        private readonly EquipmentRentalDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuditService _auditService;
        private readonly INotificationService _notificationService;

        public RentalTransactionsController(EquipmentRentalDBContext context, UserManager<IdentityUser> userManager, IAuditService auditService, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _auditService = auditService;
            _notificationService = notificationService;
        }

        // GET: RentalTransactions
        public async Task<IActionResult> Index(string customerNameSearch, string paymentStatusFilter)
        {
            var rentalTransactionsQuery = _context.RentalTransactions
                .Include(r => r.RentalRequest)
                    .ThenInclude(rr => rr.Customer)
                .Include(r => r.AssignedEquipment)
                .Include(r => r.Documents)
                .AsQueryable();

            // If the user is in the Customer role, only show their own rental transactions
            if (User.IsInRole("Customer"))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    var userEmail = currentUser.Email;
                    rentalTransactionsQuery = rentalTransactionsQuery
                        .Where(r => r.RentalRequest.Customer.Email == userEmail);
                }
            }
            else
            {
                // Filter by customer name (only for Admin/Manager)
                if (!string.IsNullOrEmpty(customerNameSearch))
                {
                    rentalTransactionsQuery = rentalTransactionsQuery
                        .Where(r => r.RentalRequest.Customer.Name.Contains(customerNameSearch));
                }
            }

            // Filter by payment status (for all roles)
            if (!string.IsNullOrEmpty(paymentStatusFilter))
            {
                rentalTransactionsQuery = rentalTransactionsQuery
                    .Where(r => r.PaymentStatus == paymentStatusFilter);
            }

            var rentalTransactions = await rentalTransactionsQuery.ToListAsync();

            var viewModel = rentalTransactions.Select(t => new RentalTransactionViewModel
            {
                Id = t.Id,
                RentalRequestId = (int)t.RentalRequestId,
                AssignedEquipmentId = (int)t.AssignedEquipmentId,
                CustomerName = t.RentalRequest.Customer.Name,
                EquipmentName = t.AssignedEquipment.Name,
                ActualRentalStartDate = t.ActualRentalStartDate,
                ReturnDate = t.ReturnDate,
                RentalPeriod = t.RentalPeriod,
                RentalFee = t.RentalFee,
                Deposit = t.Deposit,
                PaymentStatus = t.PaymentStatus,
                CreatedAt = (DateTime)t.CreatedAt,
                Documents = t.Documents.Select(d => new DocumentViewModel
                {
                    Id = d.Id,
                    FileName = d.FileName,
                    FileType = d.FileType,
                    FileData = d.FileData,
                    UploadedAt = (DateTime)d.UploadedAt
                }).ToList()
            }).ToList();

            return View(viewModel);
        }



        // GET: RentalTransaction/Create
        [Authorize(Policy = "RequireManagerRole")]
        public IActionResult Create()
        {
            // Get rental requests
            var rentalRequests = _context.RentalRequests
                .Select(r => new
                {
                    RentalRequestId = r.Id,
                    Display = r.Id.ToString()
                })
                .ToList();

            // Populate the RentalRequest dropdown with just the ID
            ViewBag.RentalRequests = new SelectList(
                rentalRequests,
                "RentalRequestId", // Value field
                "Display"      // Display field
            );

            return View();
        }






        [HttpPost]
        [Authorize(Policy = "RequireManagerRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalTransactionCreateViewModel model)
        {
            Console.WriteLine("Entering Create action...");
            Console.WriteLine($"Model state is valid: {ModelState.IsValid}");

            try
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Model is valid. Proceeding to create RentalTransaction.");
                    Console.WriteLine($"RentalRequestId: {model.RentalRequestId}, AssignedEquipmentId: {model.AssignedEquipmentId}, CustomerId: {model.CustomerId} ,ActualRentalStartDate: {model.ActualRentalStartDate}, ReturnDate: {model.ReturnDate}, RentalPeriod: {model.RentalPeriod}, RentalFee: {model.RentalFee}, Deposit: {model.Deposit}, PaymentStatus: {model.PaymentStatus}");

                    // Create the rental transaction record
                    var rentalTransaction = new RentalTransaction
                    {
                        RentalRequestId = model.RentalRequestId,
                        AssignedEquipmentId = model.AssignedEquipmentId,
                        ActualRentalStartDate = model.ActualRentalStartDate,
                        ReturnDate = model.ReturnDate,
                        RentalPeriod = model.RentalPeriod,
                        RentalFee = model.RentalFee,
                        Deposit = model.Deposit,
                        PaymentStatus = model.PaymentStatus,
                        CreatedAt = DateTime.Now,
                        CustomerId = model.CustomerId
                    };

                    _context.RentalTransactions.Add(rentalTransaction);
                    await _context.SaveChangesAsync();

                    // Handle file uploads
                    if (model.Files != null && model.Files.Count > 0)
                    {
                        Console.WriteLine($"Files found: {model.Files.Count}.  Processing files...");
                        foreach (var file in model.Files)
                        {
                            string fileType = file.ContentType;
                            const int maxFileTypeLength = 255;
                            if (fileType.Length > maxFileTypeLength)
                            {
                                fileType = fileType.Substring(0, maxFileTypeLength);
                                Console.WriteLine($"File type was truncated to: {fileType}.");
                            }
                            Console.WriteLine($"Processing file: {file.FileName}, ContentType: {fileType}, Length: {file.Length} bytes");
                            var document = new Document
                            {
                                RentalTransactionId = rentalTransaction.Id,
                                FileName = file.FileName,
                                FileType = fileType,
                                FileData = await ConvertFileToBytes(file),
                                UploadedAt = DateTime.Now
                            };
                            _context.Documents.Add(document);
                            Console.WriteLine($"Document object created for file: {file.FileName}. Added to context.");
                        }
                        await _context.SaveChangesAsync();
                        Console.WriteLine("SaveChangesAsync for Documents completed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No files to process.");
                    }

                    // --- ADD AUDIT LOG & NOTIFICATIONS ---
                    var currentUserEmail = User.Identity?.Name;
                    var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
                    if (currentUser != null)
                    {
                        await _auditService.LogActivityAsync(
                            "Rental Transaction Creation",
                            $"Rental transaction ID '{rentalTransaction.Id}' created for Rental Request ID '{model.RentalRequestId}'.",
                            currentUser.Id
                        );

                        // Notify current user (manager)
                        await _notificationService.CreateNotificationAsync(
                            currentUser.Id,
                            "Rental Transaction Created",
                            $"You created a new rental transaction ID '{rentalTransaction.Id}'."
                        );

                        // Notify the customer
                        await _notificationService.CreateNotificationAsync(
                            (int)rentalTransaction.CustomerId,
                            "Rental Transaction Created",
                            $"A rental transaction has been created for your request ID '{model.RentalRequestId}'."
                        );
                    }
                    // ---------------------------------------

                    TempData["SuccessMessage"] = "Rental transaction created successfully.";
                    Console.WriteLine("Redirecting to Index action.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Console.WriteLine("Model is invalid. Dumping ModelState errors:");
                    foreach (var keyValuePair in ModelState)
                    {
                        var key = keyValuePair.Key;
                        var state = keyValuePair.Value;

                        if (state.Errors.Any())
                        {
                            Console.WriteLine($"Key: {key}, Errors: {string.Join(", ", state.Errors.Select(e => e.ErrorMessage))}");
                        }
                    }
                    Console.WriteLine("Returning to View with model errors.");
                }

                ViewBag.RentalRequests = new SelectList(
                    _context.RentalRequests.Include(r => r.Customer),
                    "Id",
                    "Customer.Name"
                );

                ViewBag.Equipments = new SelectList(_context.Equipment, "Id", "Name");
                Console.WriteLine("Returning to View with model errors.");
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create action: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                ModelState.AddModelError(string.Empty, "An error occurred while creating the rental transaction. Please try again.");

                ViewBag.RentalRequests = new SelectList(
                    _context.RentalRequests.Include(r => r.Customer),
                    "Id",
                    "Customer.Name"
                );

                ViewBag.Equipments = new SelectList(_context.Equipment, "Id", "Name");
                Console.WriteLine("Returning to View after error.");
                return View(model);
            }
        }


        private async Task<byte[]> ConvertFileToBytes(IFormFile file)
        {
            Console.WriteLine($"Entering ConvertFileToBytes for file: {file.FileName}, ContentType: {file.ContentType}, Length: {file.Length} bytes");
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    Console.WriteLine("Copying file data to memory stream...");
                    await file.CopyToAsync(memoryStream);
                    Console.WriteLine("File data copied to memory stream.  Getting byte array...");
                    var byteArray = memoryStream.ToArray();
                    Console.WriteLine($"File data converted to byte array. Length: {byteArray.Length}");
                    return byteArray;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ConvertFileToBytes: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                // Consider how you want to handle this error. Returning null might cause issues later.
                // You might want to re-throw the exception or handle it differently based on your application's needs.
                Console.WriteLine("Returning null from ConvertFileToBytes after error.");
                return null;
            }
        }





        public async Task<IActionResult> Download(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            return File(document.FileData, document.FileType, document.FileName);
        }

        [HttpGet]
        public async Task<IActionResult> GetRentalRequestDetails(int id) // Changed parameter name to 'id' to match the AJAX data
        {
            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer.Name,
                    EquipmentId = r.EquipmentId,
                    EquipmentName = r.Equipment.Name
                })
                .FirstOrDefaultAsync();

            if (rentalRequest == null)
            {
                return Json(null); // Or return NotFound() if you prefer
            }

            return Json(rentalRequest);
        }


        // GET: RentalTransaction/Edit/5
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var rentalTransaction = await _context.RentalTransactions
                .Include(r => r.RentalRequest)
                .Include(r => r.AssignedEquipment)
                .Include(r => r.Customer)
                .Include(r => r.Documents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rentalTransaction == null) return NotFound();

            // Populate ViewBag with display names
            ViewBag.CustomerName = rentalTransaction.Customer?.Name;
            ViewBag.RentalRequestId = rentalTransaction.RentalRequestId;
            ViewBag.AssignedEquipmentName = rentalTransaction.AssignedEquipment?.Name;

            var model = new RentalTransactionEditViewModel
            {
                Id = rentalTransaction.Id,
                RentalRequestId = (int)rentalTransaction.RentalRequestId,
                AssignedEquipmentId = (int)rentalTransaction.AssignedEquipmentId,
                CustomerId = (int)rentalTransaction.CustomerId,
                ActualRentalStartDate = rentalTransaction.ActualRentalStartDate,
                ReturnDate = rentalTransaction.ReturnDate,
                RentalPeriod = rentalTransaction.RentalPeriod,
                RentalFee = rentalTransaction.RentalFee,
                Deposit = rentalTransaction.Deposit,
                PaymentStatus = rentalTransaction.PaymentStatus,
                ExistingDocuments = rentalTransaction.Documents.Select(d => new DocumentViewModel
                {
                    Id = d.Id,
                    FileName = d.FileName,
                    FileType = d.FileType,
                    UploadedAt = (DateTime)d.UploadedAt
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "RequireManagerRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentalTransactionEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var rentalTransaction = await _context.RentalTransactions
                        .Include(r => r.Documents)
                        .Include(r => r.RentalRequest)
                            .ThenInclude(rr => rr.Customer)
                        .Include(r => r.AssignedEquipment)
                        .FirstOrDefaultAsync(r => r.Id == id);

                    if (rentalTransaction == null) return NotFound();

                    // Update editable fields
                    rentalTransaction.ActualRentalStartDate = model.ActualRentalStartDate;
                    rentalTransaction.ReturnDate = model.ReturnDate;
                    rentalTransaction.RentalPeriod = model.RentalPeriod;
                    rentalTransaction.RentalFee = model.RentalFee;
                    rentalTransaction.Deposit = model.Deposit;
                    rentalTransaction.PaymentStatus = model.PaymentStatus;

                    // File handling
                    if (model.Files != null && model.Files.Count > 0)
                    {
                        if (rentalTransaction.Documents.Any())
                        {
                            _context.Documents.RemoveRange(rentalTransaction.Documents);
                        }

                        foreach (var file in model.Files)
                        {
                            var document = new Document
                            {
                                RentalTransactionId = rentalTransaction.Id,
                                FileName = file.FileName,
                                FileType = file.ContentType.Length > 255
                                    ? file.ContentType.Substring(0, 255)
                                    : file.ContentType,
                                FileData = await ConvertFileToBytes(file),
                                UploadedAt = DateTime.Now
                            };
                            _context.Documents.Add(document);
                        }
                    }

                    _context.Update(rentalTransaction);
                    await _context.SaveChangesAsync();

                    // Get current user
                    var userEmail = User.Identity?.Name;
                    var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                    if (currentUser != null)
                    {
                        // Audit log
                        await _auditService.LogActivityAsync(
                            "Rental Transaction Edited",
                            $"Rental transaction (ID: {rentalTransaction.Id}) edited by {currentUser.Name}.",
                            currentUser.Id
                        );

                        // Notify current user (manager/admin)
                        await _notificationService.CreateNotificationAsync(
                            currentUser.Id,
                            "Rental Transaction Edited",
                            $"You edited rental transaction ID {rentalTransaction.Id} for equipment '{rentalTransaction.AssignedEquipment?.Name}'."
                        );

                        // Notify customer
                        if (rentalTransaction.RentalRequest?.Customer != null)
                        {
                            await _notificationService.CreateNotificationAsync(
                                (int)rentalTransaction.RentalRequest.CustomerId,
                                "Your Rental Transaction Was Updated",
                                $"Your rental transaction (ID: {rentalTransaction.Id}) for equipment '{rentalTransaction.AssignedEquipment?.Name}' was updated by {currentUser.Name}."
                            );
                        }
                    }

                    // <-- Add TempData success message here -->
                    TempData["SuccessMessage"] = $"Rental transaction ID {rentalTransaction.Id} was updated successfully.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalTransactionExists(model.Id)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving.");
                }
            }

            // Reload documents if validation fails
            model.ExistingDocuments = await _context.Documents
                .Where(d => d.RentalTransactionId == id)
                .Select(d => new DocumentViewModel
                {
                    Id = d.Id,
                    FileName = d.FileName,
                    FileType = d.FileType,
                    UploadedAt = (DateTime)d.UploadedAt
                })
                .ToListAsync();

            return View(model);
        }


        private bool RentalTransactionExists(int id)
        {
            Console.WriteLine($"Checking if RentalTransaction exists with id: {id}");
            return _context.RentalTransactions.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var transaction = await _context.RentalTransactions
                .Include(r => r.RentalRequest)
                .Include(r => r.AssignedEquipment)
                .Include(r => r.Customer)
                .Include(r => r.Documents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (transaction == null) return NotFound();

            var viewModel = new RentalTransactionViewModel
            {
                Id = transaction.Id,
                RentalRequestId = (int)transaction.RentalRequestId,
                AssignedEquipmentId = (int)transaction.AssignedEquipmentId,
                CustomerName = transaction.RentalRequest.Customer.Name,
                EquipmentName = transaction.AssignedEquipment.Name,
                ActualRentalStartDate = transaction.ActualRentalStartDate,
                ReturnDate = transaction.ReturnDate,
                RentalPeriod = transaction.RentalPeriod,
                RentalFee = transaction.RentalFee,
                Deposit = transaction.Deposit,
                PaymentStatus = transaction.PaymentStatus,
                CreatedAt = (DateTime)transaction.CreatedAt,
                Documents = transaction.Documents.Select(d => new DocumentViewModel
                {
                    Id = d.Id,
                    FileName = d.FileName,
                    FileType = d.FileType,
                    UploadedAt = (DateTime)d.UploadedAt
                }).ToList()
            };

            return View(viewModel);
        }


        // GET: RentalTransaction/Delete/5
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalTransaction = await _context.RentalTransactions
                .Include(r => r.RentalRequest)
                .ThenInclude(rr => rr.Customer)
                .Include(r => r.AssignedEquipment)
                .Include(r => r.Documents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rentalTransaction == null)
            {
                return NotFound();
            }

            var viewModel = new RentalTransactionViewModel
            {
                Id = rentalTransaction.Id,
                RentalRequestId = (int)rentalTransaction.RentalRequestId,
                CustomerName = rentalTransaction.RentalRequest.Customer.Name,
                EquipmentName = rentalTransaction.AssignedEquipment.Name,
                ActualRentalStartDate = rentalTransaction.ActualRentalStartDate,
                ReturnDate = rentalTransaction.ReturnDate,
                RentalPeriod = rentalTransaction.RentalPeriod,
                RentalFee = rentalTransaction.RentalFee,
                Deposit = rentalTransaction.Deposit,
                PaymentStatus = rentalTransaction.PaymentStatus,
                CreatedAt = (DateTime)rentalTransaction.CreatedAt,
                Documents = rentalTransaction.Documents.Select(d => new DocumentViewModel
                {
                    Id = d.Id,
                    FileName = d.FileName,
                    FileType = d.FileType,
                    UploadedAt = (DateTime)d.UploadedAt
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: RentalTransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "RequireManagerRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentalTransaction = await _context.RentalTransactions
                .Include(r => r.RentalRequest)
                    .ThenInclude(rr => rr.Customer)
                .Include(r => r.AssignedEquipment)
                .Include(r => r.Documents)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rentalTransaction == null)
            {
                TempData["ErrorMessage"] = "Rental transaction not found.";
                return NotFound();
            }

            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "Current user not found.";
                return NotFound();
            }

            try
            {
                // Remove associated documents first
                if (rentalTransaction.Documents.Any())
                {
                    _context.Documents.RemoveRange(rentalTransaction.Documents);
                }

                _context.RentalTransactions.Remove(rentalTransaction);
                await _context.SaveChangesAsync();

                // Log audit
                await _auditService.LogActivityAsync(
                    "Rental Transaction Deleted",
                    $"Rental transaction (ID: {rentalTransaction.Id}) was deleted by {currentUser.Name}.",
                    currentUser.Id
                );

                // Notify customer
                if (rentalTransaction.RentalRequest?.Customer != null)
                {
                    await _notificationService.CreateNotificationAsync(
                        (int)rentalTransaction.RentalRequest.CustomerId,
                        "Rental Transaction Deleted",
                        $"Your rental transaction for equipment '{rentalTransaction.AssignedEquipment?.Name}' has been deleted by {currentUser.Name}."
                    );
                }

                // Notify the manager who deleted it
                await _notificationService.CreateNotificationAsync(
                    currentUser.Id,
                    "You Deleted a Rental Transaction",
                    $"You deleted rental transaction (ID: {rentalTransaction.Id}) for equipment '{rentalTransaction.AssignedEquipment?.Name}'."
                );

                TempData["SuccessMessage"] = "Rental transaction deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Delete failed: {ex.Message}");
                ModelState.AddModelError("", "Unable to delete. This transaction might have related data.");

                return View(await _context.RentalTransactions
                    .Include(r => r.RentalRequest)
                        .ThenInclude(rr => rr.Customer)
                    .Include(r => r.AssignedEquipment)
                    .FirstOrDefaultAsync(m => m.Id == id));
            }
        }



        // GET: RentalTransaction/Payment/5
        public async Task<IActionResult> Payment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalTransaction = await _context.RentalTransactions
                .Include(r => r.RentalRequest)
                    .ThenInclude(rr => rr.Customer)
                .Include(r => r.AssignedEquipment)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rentalTransaction == null)
            {
                return NotFound();
            }

            var viewModel = new RentalTransactionViewModel
            {
                Id = rentalTransaction.Id,
                CustomerName = rentalTransaction.RentalRequest.Customer.Name,
                EquipmentName = rentalTransaction.AssignedEquipment.Name,
                ActualRentalStartDate = rentalTransaction.ActualRentalStartDate,
                ReturnDate = rentalTransaction.ReturnDate,
                RentalFee = rentalTransaction.RentalFee,
                Deposit = rentalTransaction.Deposit,
                PaymentStatus = rentalTransaction.PaymentStatus
            };

            return View(viewModel);
        }

        // POST: RentalTransaction/Payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(int id)
        {
            var rentalTransaction = await _context.RentalTransactions
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rentalTransaction == null)
            {
                return NotFound();
            }

            // Update payment status based on return date
            if (rentalTransaction.ReturnDate < DateTime.Today)
            {
                rentalTransaction.PaymentStatus = "Overdue";
            }
            else
            {
                rentalTransaction.PaymentStatus = "Paid";
            }

            _context.Update(rentalTransaction);
            await _context.SaveChangesAsync();

            // Add success message
            TempData["SuccessMessage"] = "Payment processed successfully!";

            return RedirectToAction(nameof(Index));
        }
    

    
        private async Task<int?> GetCurrentUserId()
        {
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            return currentUser?.Id;
        }

    }
}

