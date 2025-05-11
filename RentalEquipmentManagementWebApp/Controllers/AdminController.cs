using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.Admin;
using RentalEquipmentManagementWebApp.Services;
using System.Data;

namespace RentalEquipmentManagementWebApp.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EquipmentRentalDBContext _context;
        private readonly IAuditService _auditService;

        public AdminController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            EquipmentRentalDBContext context,
            IAuditService auditService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardViewModel = new DashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalEquipment = await _context.Equipment.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalRentalRequests = await _context.RentalRequests.CountAsync(),
                TotalRentalTransactions = await _context.RentalTransactions.CountAsync(),
                TotalReturnRecords = await _context.ReturnRecords.CountAsync(),
                RecentLogs = await _auditService.GetRecentLogsAsync(10)
            };

            return View(dashboardViewModel);
        }

        public async Task<IActionResult> Dashboard()
        {
            // Get counts for dashboard
            var pendingRequests = await _context.RentalRequests.CountAsync(r => r.Status == "Pending");
            var approvedRequests = await _context.RentalRequests.CountAsync(r => r.Status == "Approved");
            var rejectedRequests = await _context.RentalRequests.CountAsync(r => r.Status == "Rejected");
            var completedRequests = await _context.RentalRequests.CountAsync(r => r.Status == "Completed");

            // Get equipment status counts
            var availableEquipment = await _context.Equipment.CountAsync(e => e.AvailabilityStatus == "Available");
            var rentedEquipment = await _context.Equipment.CountAsync(e => e.AvailabilityStatus == "Rented");
            var maintenanceEquipment = await _context.Equipment.CountAsync(e => e.AvailabilityStatus == "Maintenance");
            var damagedEquipment = await _context.Equipment.CountAsync(e => e.ConditionStatus == "Damaged");

            // Get category distribution
            var categoryDistribution = await _context.Categories
                .Select(c => new CategoryStatViewModel
                {
                    CategoryName = c.Name,
                    EquipmentCount = c.Equipment.Count
                })
                .ToListAsync();

            // Get recent transactions
            var recentTransactions = await _context.RentalTransactions
                .Include(rt => rt.Customer)
                .Include(rt => rt.AssignedEquipment)
                .OrderByDescending(rt => rt.CreatedAt)
                .Take(5)
                .Select(rt => new RecentTransactionViewModel
                {
                    Id = rt.Id,
                    CustomerName = rt.Customer.Name,
                    EquipmentName = rt.AssignedEquipment.Name,
                    RentalDate = rt.ActualRentalStartDate,
                    DueDate = rt.ReturnDate,
                    PaymentStatus = rt.PaymentStatus ?? "Unknown"
                })
                .ToListAsync();

            // Calculate financial summary
            var totalRevenue = await _context.RentalTransactions
                .SumAsync(rt => (decimal)rt.RentalFee);

            var pendingPayments = await _context.RentalTransactions
                .Where(rt => rt.PaymentStatus == "Pending")
                .SumAsync(rt => (decimal)rt.RentalFee);

            var additionalCharges = await _context.ReturnRecords
                .SumAsync(rr => (decimal)(rr.AdditionalCharges + rr.LateReturnFee));

            var dashboardViewModel = new AdminDashboardViewModel
            {
                RequestStatusCounts = new Dictionary<string, int>
                {
                    { "Pending", pendingRequests },
                    { "Approved", approvedRequests },
                    { "Rejected", rejectedRequests },
                    { "Completed", completedRequests }
                },
                EquipmentStatusCounts = new Dictionary<string, int>
                {
                    { "Available", availableEquipment },
                    { "Rented", rentedEquipment },
                    { "Maintenance", maintenanceEquipment },
                    { "Damaged", damagedEquipment }
                },
                CategoryDistribution = categoryDistribution,
                RecentTransactions = recentTransactions,
                FinancialSummary = new FinancialSummaryViewModel
                {
                    TotalRevenue = totalRevenue,
                    PendingPayments = pendingPayments,
                    AdditionalCharges = additionalCharges
                }
            };

            return View(dashboardViewModel);
        }

        public async Task<IActionResult> Logs(DateTime? startDate = null, DateTime? endDate = null, string? actionType = null, int? userId = null)
        {
            ViewBag.ActionTypes = await _context.Logs
                .Select(l => l.Action)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync();

            ViewBag.Users = await _context.Users
                .OrderBy(u => u.Name)
                .Select(u => new { u.Id, u.Name })
                .ToListAsync();

            var logs = await _auditService.GetLogsAsync(startDate, endDate, actionType, userId);
            return View(logs);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            var viewModel = new List<UserManagementViewModel>();

            foreach (var user in users)
            {
                var identityUser = await _userManager.FindByEmailAsync(user.Email);
                var roles = identityUser != null ? await _userManager.GetRolesAsync(identityUser) : new List<string>();

                viewModel.Add(new UserManagementViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,
                    IdentityUserId = identityUser?.Id
                });
            }

            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            var roles = identityUser != null ? await _userManager.GetRolesAsync(identityUser) : new List<string>();

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IdentityUserId = identityUser?.Id
            };

            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(model);
            }

            var user = await _context.Users.FindAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            if (identityUser == null)
            {
                return NotFound();
            }

            // Update user properties
            user.Name = model.Name;
            
            // Only update email if it has changed
            if (user.Email != model.Email)
            {
                // Update identity user email
                var setEmailResult = await _userManager.SetEmailAsync(identityUser, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    return View(model);
                }

                var setUserNameResult = await _userManager.SetUserNameAsync(identityUser, model.Email);
                if (!setUserNameResult.Succeeded)
                {
                    foreach (var error in setUserNameResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    return View(model);
                }

                // Update custom user email
                user.Email = model.Email;
            }

            // Update role if it has changed
            if (user.Role != model.Role)
            {
                // Remove from current roles
                var currentRoles = await _userManager.GetRolesAsync(identityUser);
                await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);

                // Add to new role
                await _userManager.AddToRoleAsync(identityUser, model.Role);

                // Update custom user role
                user.Role = model.Role;
            }

            // Save changes to custom user
            _context.Update(user);
            await _context.SaveChangesAsync();

            // Log the user update
            await _auditService.LogActivityAsync("User Update", $"User {user.Email} was updated by administrator", user.Id);

            // Reset password if requested
            if (model.ResetPassword)
            {
                var newPassword = "Password@123"; // Default password
                var token = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                var resetResult = await _userManager.ResetPasswordAsync(identityUser, token, newPassword);

                if (resetResult.Succeeded)
                {
                    // Update custom user password hash
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(identityUser, newPassword);
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    // Log the password reset
                    await _auditService.LogActivityAsync("Password Reset", $"Password for user {user.Email} was reset by administrator", user.Id);

                    TempData["StatusMessage"] = $"User updated successfully. Password has been reset to: {newPassword}";
                }
                else
                {
                    foreach (var error in resetResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    return View(model);
                }
            }
            else
            {
                TempData["StatusMessage"] = "User updated successfully.";
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            if (identityUser != null)
            {
                var result = await _userManager.DeleteAsync(identityUser);
                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = "Failed to delete the identity user.";
                    return RedirectToAction(nameof(Users));
                }
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            // Log the user deletion
            await _auditService.LogActivityAsync("User Deletion", $"User {user.Email} was deleted by administrator");

            TempData["StatusMessage"] = "User deleted successfully.";
            return RedirectToAction(nameof(Users));
        }
    }
}
