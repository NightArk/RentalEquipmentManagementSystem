using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementSystem_WebApplication.Models.ViewModels;
using System.Security.Claims;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly EquipmentRentalDBContext _context; // Replace with your actual DbContext


        public ProfileController(EquipmentRentalDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = GetCurrentUserIdAsInt();
            if (!currentUserId.HasValue)
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(currentUserId.Value);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserProfileViewModel // Changed to CreateUserViewModel
            {
                Name = user.Name,
                Email = user.Email,
                // You wouldn't typically populate Password or ConfirmPassword here for display
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Edit()
        {
            var currentUserId = GetCurrentUserIdAsInt();
            if (!currentUserId.HasValue)
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(currentUserId.Value);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                // Don't pre-populate password fields for security
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserProfileViewModel model)
        {
            // Remove validation for NewPassword and ConfirmNewPassword if they are empty
            if (string.IsNullOrEmpty(model.NewPassword) && string.IsNullOrEmpty(model.ConfirmNewPassword))
            {
                ModelState.Remove("NewPassword");
                ModelState.Remove("ConfirmNewPassword");
            }
            // If only one password field is filled, add an error
            else if (!string.IsNullOrEmpty(model.NewPassword) && string.IsNullOrEmpty(model.ConfirmNewPassword))
            {
                ModelState.AddModelError("ConfirmNewPassword", "Confirm New Password is required if you enter a new password.");
            }
            else if (string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.ConfirmNewPassword))
            {
                ModelState.AddModelError("NewPassword", "New Password is required if you enter a confirmation password.");
            }

            // Check for email uniqueness
            if (await _context.Users.AnyAsync(u => u.Email == model.Email && u.Id != model.Id))
            {
                ModelState.AddModelError("Email", "This email address is already in use.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUserId = GetCurrentUserIdAsInt();
            if (!currentUserId.HasValue || currentUserId.Value != model.Id)
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(currentUserId.Value);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = model.Name;
            user.Email = model.Email;
            // Update other profile fields

            // Handle optional password change
            if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.ConfirmNewPassword)
            {
                // Hash the new password using PasswordHasher
                var hasher = new PasswordHasher<User>();
                user.PasswordHash = hasher.HashPassword(user, model.NewPassword);
            }
            // If password fields are empty, we do nothing, keeping the old password

            _context.Update(user);
            await _context.SaveChangesAsync();

            ViewBag.SuccessMessage = "Profile updated successfully.";
            return RedirectToAction("Index");
        }

        private int? GetCurrentUserIdAsInt()
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
