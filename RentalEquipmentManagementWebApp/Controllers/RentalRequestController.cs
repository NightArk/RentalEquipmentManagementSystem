using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementSystem_WebApplication.Models.ViewModels;


namespace RentalEquipmentManagementWebApp.Controllers
{
    public class RentalRequestController : Controller
    {

        private readonly EquipmentRentalDBContext _context;

        public RentalRequestController(EquipmentRentalDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string searchString, string statusFilter)
        {
            IQueryable<RentalRequest> rentalRequestsQuery;
            bool isAdminOrManager = User.IsInRole("Admin") || User.IsInRole("Manager");
            int? customerId = GetCurrentUserId();

            if (isAdminOrManager)
            {
                rentalRequestsQuery = _context.RentalRequests
                    .Include(r => r.Customer)
                    .Include(r => r.Equipment)
                    .Where(r => r.Status == "Rejected" || r.Status == "Approved" || r.Status == "Pending"); // Always filter by these statuses
            }
            else if (customerId.HasValue)
            {
                rentalRequestsQuery = _context.RentalRequests
                    .Where(r => r.CustomerId == customerId)
                    .Include(r => r.Equipment)
                    .Where(r => r.Status == "Rejected" || r.Status == "Approved" || r.Status == "Pending"); // Always filter by these statuses
            }
            else
            {
                return Unauthorized();
               
            }

            // Search functionality
            if (!string.IsNullOrEmpty(searchString))
            {
                rentalRequestsQuery = rentalRequestsQuery.Where(r =>
                    (isAdminOrManager && r.Customer != null && r.Customer.Name != null && r.Customer.Name.Contains(searchString)) ||
                    (r.Equipment != null && r.Equipment.Name != null && r.Equipment.Name.Contains(searchString))
                );
            }

            // Filter by status (dropdown filter)
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                rentalRequestsQuery = rentalRequestsQuery.Where(r => r.Status == statusFilter);
            }

            // Create the ViewModel
            var viewModel = new RentalRequestIndexViewModel
            {
                RentalRequests = await rentalRequestsQuery
                    .Select(r => new RentalRequestListItemViewModel
                    {
                        Id = r.Id,
                        CreatedAt = r.CreatedAt,
                        EquipmentName = r.Equipment.Name,
                        RentalStartDate = r.RentalStartDate,
                        ReturnDate = r.ReturnDate,
                        TotalCost = r.TotalCost,
                        Status = r.Status,
                        CustomerName = isAdminOrManager ? r.Customer.Name : null,
                        Description = r.Description // Populate the Description

                    })
                    .ToListAsync(),
                SearchString = searchString,
                StatusFilter = statusFilter,
                StatusList = new List<string> { "All", "Rejected", "Approved", "Pending" } // Static list for the dropdown
            };

            ViewBag.IsAdminOrManager = isAdminOrManager;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Fetch only available equipment for the dropdown
            ViewBag.Equipment = _context.Equipment
                .Where(e => e.AvailabilityStatus == "Available")
                .ToList();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CalculateCost(int equipmentId, DateTime startDate, DateTime endDate)
        {
            var equipment = await _context.Equipment.FindAsync(equipmentId);

            if (equipment == null)
            {
                return Json(new { totalCost = 0 });
            }

            // Calculate the duration, adding one day to include the end date
            TimeSpan rentalDuration = endDate.Date.AddDays(1) - startDate.Date;
            int rentalDays = (int)rentalDuration.TotalDays;

            // Ensure at least one day
            if (rentalDays < 1)
            {
                rentalDays = 1;
            }

            decimal totalCost = equipment.RentalPrice * rentalDays;

            return Json(new { totalCost = totalCost });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalRequest model)
        {
            if (ModelState.IsValid)
            {
                if (model.RentalStartDate < DateTime.Today.Date)
                {
                    ModelState.AddModelError("RentalStartDate", "Rental start date cannot be in the past.");
                }

                if (model.ReturnDate < DateTime.Today.Date)
                {
                    ModelState.AddModelError("ReturnDate", "Return date cannot be in the past.");
                }

                if (model.ReturnDate <= model.RentalStartDate)
                {
                    ModelState.AddModelError("ReturnDate", "Return date must be after the start date.");
                }

                if (!ModelState.IsValid) // Re-check ModelState after custom date validation
                {
                    ViewBag.Equipment = _context.Equipment.Where(e => e.AvailabilityStatus == "Available").ToList();
                    return View(model);
                }

                int? customerId = GetCurrentUserId();

                if (!customerId.HasValue)
                {
                    return Unauthorized();
                }

                var selectedEquipment = await _context.Equipment.FindAsync(model.EquipmentId);

                if (selectedEquipment == null)
                {
                    ModelState.AddModelError("EquipmentId", "Invalid equipment selected.");
                    ViewBag.Equipment = _context.Equipment.Where(e => e.AvailabilityStatus == "Available").ToList();
                    return View(model);
                }

                TimeSpan rentalDuration = model.ReturnDate - model.RentalStartDate;
                int rentalDays = (int)rentalDuration.TotalDays;

                if (rentalDays < 1)
                {
                    ModelState.AddModelError("ReturnDate", "Return date must be after the start date.");
                    ViewBag.Equipment = _context.Equipment.Where(e => e.AvailabilityStatus == "Available").ToList();
                    return View(model);
                }

                model.TotalCost = selectedEquipment.RentalPrice * rentalDays;
                model.CustomerId = customerId.Value;
                model.CreatedAt = DateTime.UtcNow;
                model.Status = "Pending";
                _context.RentalRequests.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Equipment = _context.Equipment.Where(e => e.AvailabilityStatus == "Available").ToList();
            return View(model);
        }




        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Equipment)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rentalRequest == null)
            {
                return NotFound();
            }

            return View(rentalRequest);
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}
