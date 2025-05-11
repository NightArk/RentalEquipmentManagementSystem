using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.RentalTransaction;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class RentalTransactionsController : Controller
    {
        private readonly EquipmentRentalDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RentalTransactionsController(EquipmentRentalDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: RentalTransactions
        public async Task<IActionResult> Index(string customerNameSearch, string paymentStatusFilter)
        {
            var rentalTransactionsQuery = _context.RentalTransactions
                .Include(r => r.RentalRequest)
                    .ThenInclude(rr => rr.Customer)
                .Include(r => r.AssignedEquipment)
                .Include(r => r.Documents) // Include documents
                .AsQueryable();

            // Filter by customer name
            if (!string.IsNullOrEmpty(customerNameSearch))
            {
                rentalTransactionsQuery = rentalTransactionsQuery
                    .Where(r => r.RentalRequest.Customer.Name.Contains(customerNameSearch));
            }

            // Filter by payment status
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






        // POST: RentalTransaction/Create
        [HttpPost]
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

                    // Save rental transaction to DB
                    _context.RentalTransactions.Add(rentalTransaction);
                    await _context.SaveChangesAsync();

                    // Handle file uploads
                    if (model.Files != null && model.Files.Count > 0)
                    {
                        Console.WriteLine($"Files found: {model.Files.Count}.  Processing files...");
                        foreach (var file in model.Files)
                        {
                            string fileType = file.ContentType;
                            // Check the length of the file type.  Use a constant for max length
                            const int maxFileTypeLength = 255;
                            if (fileType.Length > maxFileTypeLength)
                            {
                                fileType = fileType.Substring(0, maxFileTypeLength); // Truncate to maxFileTypeLength characters
                                Console.WriteLine($"File type was truncated to: {fileType}.  Truncated value: {fileType}"); // Include truncated value in log
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
                            Console.WriteLine($"Document object created for file: {file.FileName}.  Added to context.");
                        }
                        await _context.SaveChangesAsync();
                        Console.WriteLine("SaveChangesAsync for Documents completed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No files to process.");
                    }

                    Console.WriteLine("Redirecting to Index action.");
                    // Redirect to the Index or another action as required
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Console.WriteLine("Model is invalid.  Dumping ModelState errors:");
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

                // Reload the dropdowns if validation fails
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
                // Log the error (consider using a proper logging framework)
                Console.WriteLine($"Error in Create action: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                // Optionally, you can add a model error to display on the page
                ModelState.AddModelError(string.Empty, "An error occurred while creating the rental transaction. Please try again.");

                // Reload the dropdowns if an error occurs
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
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine($"Entering Edit action with id: {id}");
            if (id == null)
            {
                Console.WriteLine("ID is null. Returning NotFound.");
                return NotFound();
            }

            var rentalTransaction = await _context.RentalTransactions
                .Include(r => r.RentalRequest)
                .Include(r => r.AssignedEquipment)
                .Include(r => r.Customer) // Include the Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalTransaction == null)
            {
                Console.WriteLine("RentalTransaction not found. Returning NotFound.");
                return NotFound();
            }

            // Populate the view model
            var model = new RentalTransactionCreateViewModel
            {
                RentalRequestId = (int)rentalTransaction.RentalRequestId,
                AssignedEquipmentId = (int)rentalTransaction.AssignedEquipmentId,
                CustomerId = (int)rentalTransaction.CustomerId, // Pass CustomerId to the model
                ActualRentalStartDate = rentalTransaction.ActualRentalStartDate,
                ReturnDate = rentalTransaction.ReturnDate,
                RentalPeriod = rentalTransaction.RentalPeriod,
                RentalFee = rentalTransaction.RentalFee,
                Deposit = rentalTransaction.Deposit,
                PaymentStatus = rentalTransaction.PaymentStatus,
                //IsRentalRequestIdReadonly = true, // These properties are not in RentalTransactionCreateViewModel
                //IsAssignedEquipmentIdReadonly = true,
                //IsCustomerIdReadonly = true,
                Files = null // Initialize Files property
            };

            // Load related data for dropdowns
            ViewBag.RentalRequests = new SelectList(_context.RentalRequests.Include(r => r.Customer), "Id", "Customer.Name", rentalTransaction.RentalRequestId);
            ViewBag.Equipments = new SelectList(_context.Equipment, "Id", "Name", rentalTransaction.AssignedEquipmentId);
            Console.WriteLine("Returning Edit view with populated model.");
            return View(model);
        }

        // POST: RentalTransaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentalTransactionCreateViewModel model)
        {
            Console.WriteLine($"Entering Edit POST action with id: {id}");
            if (id != model.Id)
            {
                Console.WriteLine("ID mismatch. Returning NotFound.");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("Model is valid. Proceeding to update RentalTransaction.");
                    var rentalTransaction = await _context.RentalTransactions.FindAsync(id);
                    if (rentalTransaction == null)
                    {
                        Console.WriteLine("RentalTransaction not found. Returning NotFound.");
                        return NotFound();
                    }

                    //update the properties
                    //rentalTransaction.RentalRequestId = model.RentalRequestId;  // Do not update these readonly fields.  These aren't readonly anymore.
                    //rentalTransaction.AssignedEquipmentId = model.AssignedEquipmentId;  // Do not update these readonly fields.  These aren't readonly anymore.
                    rentalTransaction.CustomerId = model.CustomerId;
                    rentalTransaction.ActualRentalStartDate = model.ActualRentalStartDate;
                    rentalTransaction.ReturnDate = model.ReturnDate;
                    rentalTransaction.RentalPeriod = model.RentalPeriod;
                    rentalTransaction.RentalFee = model.RentalFee;
                    rentalTransaction.Deposit = model.Deposit;
                    rentalTransaction.PaymentStatus = model.PaymentStatus;

                    // Handle file uploads
                    if (model.Files != null && model.Files.Count > 0)
                    {
                        Console.WriteLine($"Files found: {model.Files.Count}.  Processing files...");
                        // Delete old documents.
                        var existingDocuments = _context.Documents.Where(d => d.RentalTransactionId == id);
                        _context.Documents.RemoveRange(existingDocuments);

                        foreach (var file in model.Files)
                        {
                            string fileType = file.ContentType;
                            //check the length of the file type.
                            const int maxFileTypeLength = 255;
                            if (fileType.Length > maxFileTypeLength)
                            {
                                fileType = fileType.Substring(0, maxFileTypeLength); // Truncate to 255 characters
                                Console.WriteLine($"File type was truncated to: {fileType}.  Truncated value: {fileType}");
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
                            Console.WriteLine($"Document object created for file: {file.FileName}.  Added to context.");
                        }
                        await _context.SaveChangesAsync();
                        Console.WriteLine("SaveChangesAsync for Documents completed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No files to process.");
                    }

                    _context.Update(rentalTransaction);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("RentalTransaction updated successfully.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalTransactionExists(model.Id))
                    {
                        Console.WriteLine("RentalTransaction not found (concurrency error). Returning NotFound.");
                        return NotFound();
                    }
                    else
                    {
                        Console.WriteLine("Concurrency error occurred. Throwing exception.");
                        throw;
                    }
                }
                Console.WriteLine("Redirecting to Index action.");
                return RedirectToAction(nameof(Index));
            }
            // If we got this far, something failed, redisplay form with validation errors
            // Reload dropdowns
            ViewBag.RentalRequests = new SelectList(_context.RentalRequests.Include(r => r.Customer), "Id", "Customer.Name", model.RentalRequestId);
            ViewBag.Equipments = new SelectList(_context.Equipment, "Id", "Name", model.AssignedEquipmentId);
            Console.WriteLine("Returning to View with model errors.");
            return View(model);
        }

        private bool RentalTransactionExists(int id)
        {
            Console.WriteLine($"Checking if RentalTransaction exists with id: {id}");
            return _context.RentalTransactions.Any(e => e.Id == id);
        }



    }
}
