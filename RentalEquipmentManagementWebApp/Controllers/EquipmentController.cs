using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LogicModels = RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.Equipment;
using RentalEquipmentManagementWebApp.Services;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly LogicModels.EquipmentRentalDBContext _context;
        private readonly IAuditService _auditService;

        public EquipmentController(LogicModels.EquipmentRentalDBContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        // GET: Equipment
        public async Task<IActionResult> Index(int? categoryId, string? searchString, string? availabilityStatus)
        {
            var equipmentQuery = _context.Equipment
                .Include(e => e.Category)
                .AsQueryable();

            // Apply category filter
            if (categoryId.HasValue && categoryId > 0)
            {
                equipmentQuery = equipmentQuery.Where(e => e.CategoryId == categoryId);
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                equipmentQuery = equipmentQuery.Where(e => 
                    e.Name.Contains(searchString) || 
                    e.Description.Contains(searchString) ||
                    e.Category.Name.Contains(searchString));
            }

            // Apply availability filter
            if (!string.IsNullOrEmpty(availabilityStatus))
            {
                equipmentQuery = equipmentQuery.Where(e => e.AvailabilityStatus == availabilityStatus);
            }

            // Get categories for filter dropdown
            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            // Add "All Categories" option
            ViewBag.Categories.Insert(0, new SelectListItem { Value = "0", Text = "All Categories" });

            // Get availability statuses for filter dropdown
            ViewBag.AvailabilityStatuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "All Statuses" },
                new SelectListItem { Value = "Available", Text = "Available" },
                new SelectListItem { Value = "Rented", Text = "Rented" },
                new SelectListItem { Value = "Maintenance", Text = "Maintenance" },
                new SelectListItem { Value = "Damaged", Text = "Damaged" }
            };

            // Set selected values for filters
            ViewBag.SelectedCategory = categoryId ?? 0;
            ViewBag.SearchString = searchString;
            ViewBag.SelectedAvailability = availabilityStatus ?? "";

            var equipment = await equipmentQuery.ToListAsync();
            return View(equipment);
        }

        // GET: Equipment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .Include(e => e.Category)
                .Include(e => e.Feedbacks)
                .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipment/Create
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            return View();
        }

        // POST: Equipment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> Create(EquipmentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var equipment = new LogicModels.Equipment
                {
                    Name = model.Name,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    RentalPrice = model.RentalPrice,
                    AvailabilityStatus = "Available",
                    ConditionStatus = "Excellent",
                    CreatedAt = DateTime.Now
                };

                _context.Add(equipment);
                await _context.SaveChangesAsync();

                // Log the equipment creation
                await _auditService.LogActivityAsync("Equipment Creation", $"Equipment '{equipment.Name}' was created");

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            return View(model);
        }

        // GET: Equipment/Edit/5
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            var model = new EquipmentEditViewModel
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Description = equipment.Description,
                CategoryId = equipment.CategoryId ?? 0,
                RentalPrice = equipment.RentalPrice,
                AvailabilityStatus = equipment.AvailabilityStatus,
                ConditionStatus = equipment.ConditionStatus
            };

            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            ViewBag.AvailabilityStatuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "Available", Text = "Available" },
                new SelectListItem { Value = "Rented", Text = "Rented" },
                new SelectListItem { Value = "Maintenance", Text = "Maintenance" },
                new SelectListItem { Value = "Damaged", Text = "Damaged" }
            };

            ViewBag.ConditionStatuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "Excellent", Text = "Excellent" },
                new SelectListItem { Value = "Good", Text = "Good" },
                new SelectListItem { Value = "Fair", Text = "Fair" },
                new SelectListItem { Value = "Poor", Text = "Poor" },
                new SelectListItem { Value = "Damaged", Text = "Damaged" }
            };

            return View(model);
        }

        // POST: Equipment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> Edit(int id, EquipmentEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var equipment = await _context.Equipment.FindAsync(id);
                    if (equipment == null)
                    {
                        return NotFound();
                    }

                    equipment.Name = model.Name;
                    equipment.Description = model.Description;
                    equipment.CategoryId = model.CategoryId;
                    equipment.RentalPrice = model.RentalPrice;
                    equipment.AvailabilityStatus = model.AvailabilityStatus;
                    equipment.ConditionStatus = model.ConditionStatus;

                    _context.Update(equipment);
                    await _context.SaveChangesAsync();

                    // Log the equipment update
                    await _auditService.LogActivityAsync("Equipment Update", $"Equipment '{equipment.Name}' was updated");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            ViewBag.AvailabilityStatuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "Available", Text = "Available" },
                new SelectListItem { Value = "Rented", Text = "Rented" },
                new SelectListItem { Value = "Maintenance", Text = "Maintenance" },
                new SelectListItem { Value = "Damaged", Text = "Damaged" }
            };

            ViewBag.ConditionStatuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "Excellent", Text = "Excellent" },
                new SelectListItem { Value = "Good", Text = "Good" },
                new SelectListItem { Value = "Fair", Text = "Fair" },
                new SelectListItem { Value = "Poor", Text = "Poor" },
                new SelectListItem { Value = "Damaged", Text = "Damaged" }
            };

            return View(model);
        }

        // GET: Equipment/Delete/5
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .Include(e => e.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            // Check if equipment is in use
            var hasRentalRequests = await _context.RentalRequests.AnyAsync(r => r.EquipmentId == id);
            var hasRentalTransactions = await _context.RentalTransactions.AnyAsync(r => r.AssignedEquipmentId == id);

            if (hasRentalRequests || hasRentalTransactions)
            {
                TempData["ErrorMessage"] = "Cannot delete equipment that is associated with rental requests or transactions.";
                return RedirectToAction(nameof(Index));
            }

            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();

            // Log the equipment deletion
            await _auditService.LogActivityAsync("Equipment Deletion", $"Equipment '{equipment.Name}' was deleted");

            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.Id == id);
        }
    }
}
