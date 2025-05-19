
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RentalEquipmentManagementLogic;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.RentalRequest;
using RentalEquipmentManagementWebApp.Services;

namespace RentalEquipmentManagementWebApp.Controllers
{
    [Authorize(Policy = "RequireAuthenticated")]
    public class RentalRequestsController : Controller
    {
        private readonly EquipmentRentalDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISharedAuthenticationService _sharedAuthService;
        private readonly IAuditService _auditService;
        private readonly INotificationService _notificationService;



        public RentalRequestsController(EquipmentRentalDBContext context, UserManager<IdentityUser> userManager, IAuditService auditService, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _sharedAuthService = new SharedAuthenticationService(context);
            _auditService = auditService;
            _notificationService = notificationService;

        }

        // GET: RentalRequests
        public async Task<IActionResult> Index(
       string statusFilter,
       DateTime? fromDate,
       DateTime? toDate,
       string equipmentSearch,
       string customerNameSearch)
        {
            var userEmail = User.Identity?.Name;
            var identityUser = await _userManager.FindByEmailAsync(userEmail);
            var userRoles = await _userManager.GetRolesAsync(identityUser);
            var isCustomerOnly = userRoles.Count == 1 && userRoles.Contains("Customer");

            var query = _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .AsQueryable();

            if (isCustomerOnly)
            {
                query = query.Where(r => r.Customer.Email == userEmail);
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
                query = query.Where(r => r.Status == statusFilter);

            if (fromDate.HasValue)
                query = query.Where(r => r.RentalStartDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(r => r.ReturnDate <= toDate.Value);

            if (!string.IsNullOrWhiteSpace(equipmentSearch))
                query = query.Where(r => r.Equipment.Name.Contains(equipmentSearch));

            if (!string.IsNullOrWhiteSpace(customerNameSearch))
                query = query.Where(r => r.Customer.Name.Contains(customerNameSearch));

            var rentalRequests = await query.Select(r => new RentalRequestViewModel
            {
                Id = r.Id,
                CustomerId = r.CustomerId ?? 0,
                CustomerName = r.Customer.Name,
                EquipmentId = r.EquipmentId ?? 0,
                EquipmentName = r.Equipment.Name,
                RentalStartDate = r.RentalStartDate,
                ReturnDate = r.ReturnDate,
                TotalCost = r.TotalCost,
                Status = r.Status,
                Description = r.Description,
                CreatedAt = r.CreatedAt ?? DateTime.MinValue,

                // Filters (optional if you want to persist values)
                StatusFilter = statusFilter,
                FromDate = fromDate,
                ToDate = toDate,
                EquipmentSearch = equipmentSearch,
                CustomerNameSearch = customerNameSearch
            }).ToListAsync();

            return View(rentalRequests);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rentalRequest == null)
            {
                return NotFound();
            }

            var viewModel = new RentalRequestViewModel
            {
                Id = rentalRequest.Id,
                CustomerId = rentalRequest.CustomerId ?? 0,
                CustomerName = rentalRequest.Customer?.Name,
                EquipmentId = rentalRequest.EquipmentId ?? 0,
                EquipmentName = rentalRequest.Equipment?.Name,
                RentalStartDate = rentalRequest.RentalStartDate,
                ReturnDate = rentalRequest.ReturnDate,
                TotalCost = rentalRequest.TotalCost,
                Status = rentalRequest.Status,
                Description = rentalRequest.Description,
                CreatedAt = rentalRequest.CreatedAt ?? DateTime.MinValue
            };

            return View(viewModel);
        }


        // GET: RentalRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get current user by email
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return NotFound();
            }

            // Determine if the current user is a Customer
            var isCustomer = currentUser.Role == "Customer";

            // Get the rental request including related data
            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Equipment)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rentalRequest == null)
            {
                return NotFound();
            }

            // If user is a customer, they can only edit their own requests
            if (isCustomer && rentalRequest.CustomerId != currentUser.Id)
            {
                return Forbid();
            }

            var viewModel = new RentalRequestEditViewModel
            {
                Id = rentalRequest.Id,
                EquipmentId = (int)rentalRequest.EquipmentId,
                EquipmentName = rentalRequest.Equipment.Name,
                CustomerId = (int)rentalRequest.CustomerId,
                CustomerName = rentalRequest.Customer.Name, // Or use Email if Name is null
                StartDate = rentalRequest.RentalStartDate,
                ReturnDate = rentalRequest.ReturnDate,
                TotalCost = rentalRequest.TotalCost,
                Description = rentalRequest.Description,
                Status = rentalRequest.Status,
                CanEditDates = !isCustomer,
                CanEditDescription = true
            };

            ViewBag.IsCustomer = isCustomer;
            ViewBag.EquipmentDailyRate = rentalRequest.Equipment.RentalPrice;

            return View(viewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentalRequestEditViewModel model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "Rental request not found.";
                return NotFound();
            }

            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "Current user not found.";
                return NotFound();
            }

            var isCustomer = currentUser.Role == "Customer";

            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Equipment)
                .Include(r => r.Customer) // Include customer for notification
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rentalRequest == null)
            {
                TempData["ErrorMessage"] = "Rental request not found.";
                return NotFound();
            }

            if (isCustomer && rentalRequest.CustomerId != currentUser.Id)
            {
                TempData["ErrorMessage"] = "You are not authorized to edit this request.";
                return Forbid();
            }

            if (isCustomer)
            {
                if (rentalRequest.Status != "Pending")
                {
                    TempData["ErrorMessage"] = "Only pending requests can be edited.";
                    return Forbid();
                }

                rentalRequest.Description = model.Description;
            }
            else
            {
                rentalRequest.RentalStartDate = model.StartDate;
                rentalRequest.ReturnDate = model.ReturnDate;
                rentalRequest.Status = model.Status;
                rentalRequest.Description = model.Description;

                if (rentalRequest.Equipment != null)
                {
                    var rentalDays = (model.ReturnDate - model.StartDate).Days;
                    rentalDays = Math.Max(rentalDays, 1);
                    rentalRequest.TotalCost = rentalDays * rentalRequest.Equipment.RentalPrice;
                }
            }

            try
            {
                await _context.SaveChangesAsync();

                await _auditService.LogActivityAsync(
                    "Rental Request Edited",
                    $"Rental request ID '{rentalRequest.Id}' was edited by {currentUser.Name}.",
                    currentUser.Id
                );

                // Notifications
                if (!isCustomer)
                {
                    await _notificationService.CreateNotificationAsync(currentUser.Id,
                        "Rental Request Updated",
                        $"You updated rental request ID '{rentalRequest.Id}'.");

                    if (rentalRequest.CustomerId != currentUser.Id)
                    {
                        await _notificationService.CreateNotificationAsync((int)rentalRequest.CustomerId,
                            "Rental Request Updated",
                            $"Your rental request ID '{rentalRequest.Id}' was updated.");
                    }
                }
                else
                {
                    await _notificationService.CreateNotificationAsync(currentUser.Id,
                        "Rental Request Updated",
                        $"You updated your rental request ID '{rentalRequest.Id}'.");
                }

                TempData["SuccessMessage"] = "Rental request updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RentalRequests.Any(r => r.Id == id))
                {
                    TempData["ErrorMessage"] = "Rental request no longer exists.";
                    return NotFound();
                }
                else
                {
                    TempData["ErrorMessage"] = "Error occurred while updating the rental request.";
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }




        // GET: RentalRequest/Delete/5
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rentalRequest == null) return NotFound();

            var viewModel = new RentalRequestViewModel
            {
                Id = rentalRequest.Id,
                CustomerId = rentalRequest.CustomerId ?? 0,
                CustomerName = rentalRequest.Customer?.Name,
                EquipmentId = rentalRequest.EquipmentId ?? 0,
                EquipmentName = rentalRequest.Equipment?.Name,
                RentalStartDate = rentalRequest.RentalStartDate,
                ReturnDate = rentalRequest.ReturnDate,
                TotalCost = rentalRequest.TotalCost,
                Status = rentalRequest.Status,
                Description = rentalRequest.Description,
                CreatedAt = rentalRequest.CreatedAt ?? DateTime.MinValue
            };

            return View(viewModel);
        }

        // POST: RentalRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "RequireManagerRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "Current user not found.";
                return NotFound();
            }

            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rentalRequest != null)
            {
                _context.RentalRequests.Remove(rentalRequest);
                await _context.SaveChangesAsync();

                // Log audit
                await _auditService.LogActivityAsync(
                    "Rental Request Deleted",
                    $"Rental request (ID: {rentalRequest.Id}) for Equipment '{rentalRequest.Equipment?.Name}' was deleted by {currentUser.Name}.",
                    currentUser.Id
                );

                // Notify the customer
                await _notificationService.CreateNotificationAsync(
                    (int)rentalRequest.CustomerId,
                    "Rental Request Deleted",
                    $"Your rental request for equipment '{rentalRequest.Equipment?.Name}' has been deleted by {currentUser.Name}."
                );

                // Notify the admin/manager who deleted it
                await _notificationService.CreateNotificationAsync(
                    currentUser.Id,
                    "You Deleted a Rental Request",
                    $"You deleted rental request (ID: {rentalRequest.Id}) for equipment '{rentalRequest.Equipment?.Name}'."
                );

                TempData["SuccessMessage"] = "Rental request deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Rental request not found.";
            }

            return RedirectToAction(nameof(Index));
        }



        // Update for the GET Create action
        public async Task<IActionResult> Create(int? equipmentId)
        {
            var userEmail = User.Identity?.Name;

            // Get current user via DbContext
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "Current user not found.";
                return NotFound();
            }

            // Get role directly from user entity
            var isCustomer = currentUser.Role == "Customer"; // Direct column access

            var model = new RentalRequestCreateViewModel
            {
                StartDate = DateTime.Today,
                ReturnDate = DateTime.Today.AddDays(1)
            };

            if (isCustomer)
            {
                ViewBag.CustomerName = currentUser.Name;
                ViewBag.CustomerId = currentUser.Id; // string ID
            }
            else
            {
                // Get customers directly from Users table
                var customers = await _context.Users
                    .Where(u => u.Role == "Customer") // Direct role column filter
                    .ToListAsync();

                var customersList = customers.Select(user => new SelectListItem
                {
                    Value = user.Id.ToString(),
                    Text = !string.IsNullOrEmpty(user.Name) ? user.Name : user.Email
                }).ToList();

                ViewBag.Customers = customersList;
            }

            // Equipment handling
            if (equipmentId.HasValue)
            {
                var equipment = await _context.Equipment.FindAsync(equipmentId.Value);
                if (equipment != null)
                {
                    model.EquipmentId = equipment.Id;
                    ViewBag.EquipmentName = equipment.Name;
                    ViewBag.EquipmentRentalPrice = equipment.RentalPrice;
                }
            }
            else
            {
                var equipmentList = await _context.Equipment
                    .Where(e => e.AvailabilityStatus == "Available")
                    .ToListAsync();
                ViewBag.Equipment = new SelectList(equipmentList, "Id", "Name");
            }

            // Add equipment prices dictionary
            var availableEquipment = await _context.Equipment
                .Where(e => e.AvailabilityStatus == "Available")
                .ToListAsync();
            ViewBag.EquipmentPrices = availableEquipment.ToDictionary(e => e.Id, e => e.RentalPrice);
            ViewBag.IsCustomer = isCustomer;
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalRequestCreateViewModel model)
        {
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "Current user not found.";
                return NotFound();
            }

            var isCustomer = currentUser.Role == "Customer";
            int customerId = isCustomer ? currentUser.Id : model.CustomerId;
            var customer = await _context.Users.FindAsync(customerId);


            if (model.ReturnDate <= model.StartDate)
            {
                ModelState.AddModelError("ReturnDate", "Return date must be after start date.");
            }

            if (ModelState.IsValid)
            {
                // Save using values from the form (TotalCost already calculated in view)
                var rentalRequest = new RentalRequest
                {
                    CustomerId = customerId,
                    EquipmentId = model.EquipmentId,
                    RentalStartDate = model.StartDate,
                    ReturnDate = model.ReturnDate,
                    TotalCost = model.TotalCost, 
                    Description = model.Description,
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                _context.RentalRequests.Add(rentalRequest);
                await _context.SaveChangesAsync();

                await _auditService.LogActivityAsync("Rental Request Creation", $"New rental request created for Equipment ID '{model.EquipmentId}'.", currentUser.Id);

                // Send notifications
                if (!isCustomer && customer != null)
                {
                    await _notificationService.CreateNotificationAsync(currentUser.Id, "Rental Request Created", $"You created a new rental request for Equipment ID '{model.EquipmentId}' for Customer: {customer.Name}.");
                    await _notificationService.CreateNotificationAsync(customerId, "Rental Request Received", $"A new rental request has been created for you for Equipment ID '{model.EquipmentId}'.");
                }
                else
                {
                    await _notificationService.CreateNotificationAsync(currentUser.Id, "Rental Request Created", $"You created a new rental request for Equipment ID '{model.EquipmentId}'.");
                }


                TempData["SuccessMessage"] = "Rental request created successfully.";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if ModelState is invalid
            if (isCustomer)
            {
                ViewBag.CustomerName = currentUser.Name;
                ViewBag.CustomerId = currentUser.Id;
            }
            else
            {
                var customers = await _context.Users
                    .Where(u => u.Role == "Customer")
                    .ToListAsync();

                ViewBag.Customers = customers.Select(user => new SelectListItem
                {
                    Value = user.Id.ToString(),
                    Text = !string.IsNullOrEmpty(user.Name) ? user.Name : user.Email,
                    Selected = user.Id == model.CustomerId
                }).ToList();
            }

            var equipmentList = await _context.Equipment
                .Where(e => e.AvailabilityStatus == "Available")
                .ToListAsync();
            ViewBag.Equipment = new SelectList(equipmentList, "Id", "Name", model.EquipmentId);

            var availableEquipmentPrices = await _context.Equipment
                .Where(e => e.AvailabilityStatus == "Available")
                .ToDictionaryAsync(e => e.Id, e => e.RentalPrice);
            ViewBag.EquipmentPrices = availableEquipmentPrices;
            ViewBag.IsCustomer = isCustomer;

            TempData["ErrorMessage"] = "Failed to create rental request. Please check the input.";
            return View(model);
        }


        private async Task<int?> GetCurrentUserId()
        {
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            return currentUser?.Id;
        }


    }
}
