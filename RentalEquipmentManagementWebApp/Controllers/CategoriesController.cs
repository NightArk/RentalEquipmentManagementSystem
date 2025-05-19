using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Services;
using System.Threading.Tasks;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly EquipmentRentalDBContext _context;
        private readonly IAuditService _auditService;
        private readonly INotificationService _notificationService;

        public CategoriesController(EquipmentRentalDBContext context, IAuditService auditService, INotificationService notificationService)
        {
            _context = context;
            _auditService = auditService;
            _notificationService = notificationService;
        }

        [Authorize]
        public IActionResult Index(string searchString)
        {
            var categories = from c in _context.Categories select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(c => c.Name.Contains(searchString));
            }

            return View(categories.ToList());
        }

        [Authorize(Policy = "RequireManagerRole")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "RequireManagerRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                var userId = await GetCurrentUserId();

                if (userId != null)
                {
                    await _auditService.LogActivityAsync("Category Creation", $"Category '{category.Name}' was created.", userId.Value);
                    await _notificationService.CreateNotificationAsync(userId.Value, "Category Created", "Category Created", $"You created category '{category.Name}'.");
                }

                TempData["SuccessMessage"] = $"Category '{category.Name}' was created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to create category. Please check the input.";
            return View(category);
        }

        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [Authorize(Policy = "RequireManagerRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Categories.Update(category);
                    await _context.SaveChangesAsync();

                    var userId = await GetCurrentUserId();

                    if (userId != null)
                    {
                        await _auditService.LogActivityAsync("Category Edit", $"Category '{category.Name}' was updated.", userId.Value);
                        await _notificationService.CreateNotificationAsync(userId.Value, "Category Updated", "Category Updated", $"You updated category '{category.Name}'.");
                    }

                    TempData["SuccessMessage"] = $"Category '{category.Name}' was updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Just rethrow here, no extra logic added
                    throw;
                }
            }

            TempData["ErrorMessage"] = "Failed to update category. Please check the input.";
            return View(category);
        }

        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "RequireManagerRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                TempData["ErrorMessage"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            var userId = await GetCurrentUserId();

            if (userId != null)
            {
                await _auditService.LogActivityAsync("Category Deletion", $"Category '{category.Name}' was deleted.", userId.Value);
                await _notificationService.CreateNotificationAsync(userId.Value, "Category Deleted", "Category Deleted", $"You deleted category '{category.Name}'.");
            }

            TempData["SuccessMessage"] = $"Category '{category.Name}' was deleted successfully.";
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
