using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LogicModels = RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.Equipment;
using RentalEquipmentManagementWebApp.Services;
using Microsoft.AspNetCore.Identity;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly LogicModels.EquipmentRentalDBContext _context;
        private readonly IAuditService _auditService;
        private readonly INotificationService _notificationService;
       




        public EquipmentController(LogicModels.EquipmentRentalDBContext context, IAuditService auditService, INotificationService notificationService)
        {
            _context = context;
            _auditService = auditService;
            _notificationService = notificationService;

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
                new SelectListItem { Value = "Under Maintenance", Text = "Under Maintenance" },
                new SelectListItem { Value = "unavailable", Text = "unavailable" }
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
                .Include(e => e.Feedbacks.Where(i => i.IsHide == false))
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
                try
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

                    var userId = await GetCurrentUserId();

                    await _auditService.LogActivityAsync("Equipment Creation", $"Equipment '{equipment.Name}' was created", userId.Value);

                    if (userId != null)
                    {
                        await _notificationService.CreateNotificationAsync(
                            userId.Value,
                            "Equipment Created",
                            "Equipment Created",
                            $"You created equipment '{equipment.Name}'."
                        );
                    }

                    TempData["SuccessMessage"] = $"Equipment '{equipment.Name}' was created successfully.";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log exception here if needed
                    TempData["ErrorMessage"] = $"Failed to create equipment: {ex.Message}";
                }
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
                new SelectListItem { Value = "Under Maintenance", Text = "Under Maintenance" },
                new SelectListItem { Value = "unavailable", Text = "unavailable" }
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
                TempData["ErrorMessage"] = "Equipment ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var equipment = await _context.Equipment.FindAsync(id);
                    if (equipment == null)
                    {
                        TempData["ErrorMessage"] = "Equipment not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    equipment.Name = model.Name;
                    equipment.Description = model.Description;
                    equipment.CategoryId = model.CategoryId;
                    equipment.RentalPrice = model.RentalPrice;
                    equipment.AvailabilityStatus = model.AvailabilityStatus;
                    equipment.ConditionStatus = model.ConditionStatus;

                    _context.Update(equipment);
                    await _context.SaveChangesAsync();

                    var userId = await GetCurrentUserId();
                    await _auditService.LogActivityAsync("Equipment Update", $"Equipment '{equipment.Name}' was updated", userId);

                    if (userId != null)
                    {
                        await _notificationService.CreateNotificationAsync(
                            userId.Value,
                            "Equipment Updated",
                            "Equipment Updated",
                            $"You updated equipment '{equipment.Name}'."
                        );
                    }

                    TempData["SuccessMessage"] = $"Equipment '{model.Name}' was updated successfully.";
                    return RedirectToAction(nameof(Index));


                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(model.Id))
                    {
                        TempData["ErrorMessage"] = "Equipment no longer exists.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Failed to update equipment: {ex.Message}";
                }
            }

            // Repopulate dropdowns and return view on failure
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
            new SelectListItem { Value = "Under Maintenance", Text = "Under Maintenance" },
            new SelectListItem { Value = "unavailable", Text = "unavailable" }
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
                TempData["ErrorMessage"] = "Equipment not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Equipment.Remove(equipment);
                await _context.SaveChangesAsync();

                var userId = await GetCurrentUserId();
                await _auditService.LogActivityAsync("Equipment Deletion", $"Equipment '{equipment.Name}' was deleted", userId);

                if (userId != null)
                {
                    await _notificationService.CreateNotificationAsync(
                      userId.Value,
                      "Equipment Deleted",    // notificationType
                      "Equipment Deleted",    // title
                      $"You deleted equipment '{equipment.Name}'."
                        );
                }

                TempData["SuccessMessage"] = $"Equipment '{equipment.Name}' was deleted successfully.";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete equipment: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.Id == id);
        }


        private async Task<int?> GetCurrentUserId()
        {
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            return currentUser?.Id;
        }
    }
}
