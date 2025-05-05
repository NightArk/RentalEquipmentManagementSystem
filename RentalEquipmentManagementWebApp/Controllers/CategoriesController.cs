using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Add this
using System.Collections.Generic; // Add this
using System.Linq; // Add this
using RentalEquipmentManagementLogic.Models; // Assuming your models are in this namespace

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly EquipmentRentalDBContext _context; // Replace with your actual database context

        public CategoriesController(EquipmentRentalDBContext context) // Inject the context
        {
            _context = context;
        }

        [Authorize] // Ensure only logged-in users can access
        public IActionResult Index(string searchString)
        {
            var categories = from c in _context.Categories select c; // Get all categories

            if (!string.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(c => c.Name.Contains(searchString)); // Filter if search string is provided
            }

            return View(categories.ToList()); // Pass the categories to the view
        }

        [Authorize(Roles = "Admin")] // Only Admins can access these
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}