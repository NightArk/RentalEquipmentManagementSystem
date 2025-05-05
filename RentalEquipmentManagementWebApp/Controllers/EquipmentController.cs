using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentalEquipmentManagementSystem_WebApplication.Models.ViewModels;

namespace RentalEquipmentManagementWebApp.Controllers
{
    [Authorize] // Requires authentication for the entire controller by default
    public class EquipmentController : Controller
    {
        private readonly EquipmentRentalDBContext _context;

        public EquipmentController(EquipmentRentalDBContext context)
        {
            _context = context;
        }

        // GET: Equipment
        public async Task<IActionResult> Index(string searchString, string availabilityFilter, string conditionFilter, int? categoryFilter)
        {
            var equipmentQuery = _context.Equipment.Include(e => e.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                equipmentQuery = equipmentQuery.Where(e => e.Name.Contains(searchString) || e.Description.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(availabilityFilter))
            {
                equipmentQuery = equipmentQuery.Where(e => e.AvailabilityStatus == availabilityFilter);
            }

            if (!string.IsNullOrEmpty(conditionFilter))
            {
                equipmentQuery = equipmentQuery.Where(e => e.ConditionStatus == conditionFilter);
            }

            if (categoryFilter.HasValue)
            {
                equipmentQuery = equipmentQuery.Where(e => e.CategoryId == categoryFilter);
            }

            var viewModel = new EquipmentIndexViewModel
            {
                Equipment = await equipmentQuery.ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
                SearchString = searchString,
                AvailabilityFilter = availabilityFilter,
                ConditionFilter = conditionFilter,
                CategoryFilter = categoryFilter
            };

            return View(viewModel);
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
                .FirstOrDefaultAsync(m => m.Id == id);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipment/Create
        [Authorize(Roles = "Admin,Manager")] // Only Admins and Managers can create equipment
        public async Task<IActionResult> Create()
        {
            var viewModel = new EquipmentCreateViewModel
            {
                Categories = await _context.Categories.ToListAsync() // Get the categories for the dropdown
            };
            return View(viewModel);
        }

        // POST: Equipment/Create
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")] // Only Admins and Managers can create equipment
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EquipmentCreateViewModel viewModel)
        {
            
                var equipment = new Equipment
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    CategoryId = viewModel.CategoryId,
                    RentalPrice = viewModel.RentalPrice,
                    AvailabilityStatus = viewModel.AvailabilityStatus,
                    ConditionStatus = viewModel.ConditionStatus,
                    CreatedAt = DateTime.Now // Set the creation timestamp
                };

                _context.Equipment.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the equipment list
            

            
        }

        [Authorize(Roles = "Admin,Manager")]
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

            var viewModel = new EquipmentCreateViewModel
            {
                Name = equipment.Name,
                Description = equipment.Description,
                CategoryId = (int)equipment.CategoryId,
                RentalPrice = equipment.RentalPrice,
                AvailabilityStatus = equipment.AvailabilityStatus,
                ConditionStatus = equipment.ConditionStatus,
                Categories = await _context.Categories.ToListAsync() 
            };

          
            return View(viewModel);
        }

        // POST: Equipment/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EquipmentCreateViewModel viewModel) // Accept EquipmentCreateViewModel
        {
            
                var equipment = await _context.Equipment.FindAsync(id); // Fetch the existing equipment to update
                if (equipment == null)
                {
                    return NotFound();
                }

                // Update the equipment properties from the ViewModel
                equipment.Name = viewModel.Name;
                equipment.Description = viewModel.Description;
                equipment.CategoryId = viewModel.CategoryId;
                equipment.RentalPrice = viewModel.RentalPrice;
                equipment.AvailabilityStatus = viewModel.AvailabilityStatus;
                equipment.ConditionStatus = viewModel.ConditionStatus;
                // Do not update CreatedAt unless that's the desired behavior

                try
                {
                    _context.Equipment.Update(equipment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

        }

        // GET: Equipment/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int? id)
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

            return View(equipment);
        }

        // POST: Equipment/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipment.Remove(equipment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.Id == id);
        }
    }
}