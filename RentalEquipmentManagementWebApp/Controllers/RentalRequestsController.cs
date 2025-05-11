
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.RentalRequest;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class RentalRequestsController : Controller
    {
        private readonly EquipmentRentalDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public RentalRequestsController(EquipmentRentalDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

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

            var query = _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .AsQueryable();

            if (userRoles.Contains("Customer"))
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

                // Pass search/filter values back to the view
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
            if (id == null) return NotFound();

            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rentalRequest == null) return NotFound();

            var userEmail = User.Identity?.Name;
            var identityUser = await _userManager.FindByEmailAsync(userEmail);
            var roles = await _userManager.GetRolesAsync(identityUser);

            bool isCustomer = roles.Contains("Customer");
            if (isCustomer && rentalRequest.Status != "Pending")
            {
                return Forbid(); // Customer trying to edit a non-pending request
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

            ViewBag.IsCustomer = isCustomer;
            return View(viewModel);
        }

        // POST: RentalRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentalRequestViewModel model)
        {
            if (id != model.Id) return NotFound();

            var rentalRequest = await _context.RentalRequests.FindAsync(id);
            if (rentalRequest == null) return NotFound();

            var userEmail = User.Identity?.Name;
            var identityUser = await _userManager.FindByEmailAsync(userEmail);
            var roles = await _userManager.GetRolesAsync(identityUser);

            bool isCustomer = roles.Contains("Customer");

            if (isCustomer && rentalRequest.Status != "Pending")
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (isCustomer)
                    {
                        rentalRequest.Description = model.Description;
                    }
                    else
                    {
                        rentalRequest.RentalStartDate = model.RentalStartDate;
                        rentalRequest.ReturnDate = model.ReturnDate;
                        rentalRequest.TotalCost = model.TotalCost;
                        rentalRequest.Status = model.Status;
                        rentalRequest.Description = model.Description;
                    }

                    _context.Update(rentalRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.RentalRequests.Any(e => e.Id == model.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            ViewBag.IsCustomer = isCustomer;
            return View(model);
        }


        // GET: RentalRequest/Delete/5
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentalRequest = await _context.RentalRequests.FindAsync(id);
            if (rentalRequest != null)
            {
                _context.RentalRequests.Remove(rentalRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: RentalRequest/Create
        public async Task<IActionResult> Create()
        {
            var userEmail = User.Identity?.Name;
            var identityUser = await _userManager.FindByEmailAsync(userEmail);
            var userRoles = await _userManager.GetRolesAsync(identityUser);

            var model = new RentalRequestCreateViewModel
            {
                // Default to the current user's Id for Customers
                // Fix for CS0029: Convert 'identityUser.Id' from 'string' to 'int' as required by the 'UserId' property in RentalRequestCreateViewModel
                UserId = int.TryParse(identityUser.Id, out var userId) ? userId : 0,
                //UserId = identityUser.Id,
                EquipmentList = await _context.Equipment.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                }).ToListAsync()
            };

            // For Admin/Manager, allow them to select a user
            if (userRoles.Contains("Administrator") || userRoles.Contains("Manager"))
            {
                model.UserList = await _userManager.Users.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                }).ToListAsync();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalRequestCreateViewModel model)
        {
            // Fix for CS1503: Convert 'model.UserId' from 'int' to 'string' as required by the method 'FindByIdAsync'
            var userRoles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(model.UserId.ToString()));

            if (ModelState.IsValid)
            {
                var rentalRequest = new RentalRequest
                {
                    CustomerId = model.UserId,  // Linking to the selected user (customer)
                    EquipmentId = model.EquipmentId,
                    RentalStartDate = model.RentalStartDate,
                    ReturnDate = model.ReturnDate,
                    TotalCost = model.TotalCost,
                    Status = "Pending",  // Default status is "Pending"
                    Description = model.Description,
                    CreatedAt = DateTime.Now
                };

                _context.RentalRequests.Add(rentalRequest);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Rental request created successfully.";
                return RedirectToAction("Index");
            }

            // Re-populate the Equipment List and User List for Admin/Manager
            model.EquipmentList = await _context.Equipment.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }).ToListAsync();

            if (userRoles.Contains("Administrator") || userRoles.Contains("Manager"))
            {
                model.UserList = await _userManager.Users.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                }).ToListAsync();
            }

            return View(model);
        }




    }
}
