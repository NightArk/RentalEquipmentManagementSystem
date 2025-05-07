using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using System.Security.Claims;
using RentalEquipmentManagementWebApp.Models.ViewModels;

namespace RentalEquipmentManagementWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly EquipmentRentalDBContext _context;
        private readonly PasswordHasher<User> _hasher;

        public AdminController(EquipmentRentalDBContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
        }

        // Action for Admin Dashboard

        public IActionResult Dashboard()
        {
            
            return View();
        }


        public async Task<IActionResult> AuditLogs()
        {


            // Fetch logs from the database, including user details, and order them by Log ID (ascending or descending)
            var logs = await _context.Logs
                .Include(l => l.User)  // Assuming Log has a User navigation property
                .OrderBy(l => l.Id)    // Sort logs by Log ID in ascending order
                .ToListAsync();

            return View(logs);  // Pass logs to the view
        }

        public async Task<IActionResult> UserManagement()
        {
            var users = await _context.Users.ToListAsync(); // Fetch all users from your database
            return View(users); // Pass the list of users to the view
        }

        // GET: Admin/CreateUser
        public IActionResult CreateUser()
        {
            return View(); // Returns the CreateUser.cshtml view
        }

        // POST: Admin/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    Role = viewModel.Role,
                    CreatedAt = DateTime.UtcNow // Or your desired creation timestamp
                };

                // Hash the password
                newUser.PasswordHash = _hasher.HashPassword(newUser, viewModel.Password);

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(UserManagement)); // Redirect to the user list
            }

            // If ModelState is not valid, return the view with validation errors
            return View(viewModel);
        }

        // GET: Admin/EditUser/{id}
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return View(viewModel);
        }

        // POST: Admin/EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel viewModel)
        {


            // Remove validation for NewPassword and ConfirmNewPassword if they are empty
            if (string.IsNullOrEmpty(viewModel.NewPassword) && string.IsNullOrEmpty(viewModel.ConfirmNewPassword))
            {
                ModelState.Remove("NewPassword");
                ModelState.Remove("ConfirmNewPassword");

            }
            // If only one password field is filled, add an error
            else if (!string.IsNullOrEmpty(viewModel.NewPassword) && string.IsNullOrEmpty(viewModel.ConfirmNewPassword))
            {
                ModelState.AddModelError("ConfirmNewPassword", "Confirm New Password is required if you enter a new password.");
            }
            else if (string.IsNullOrEmpty(viewModel.NewPassword) && !string.IsNullOrEmpty(viewModel.ConfirmNewPassword))
            {
                ModelState.AddModelError("NewPassword", "New Password is required if you enter a confirmation password.");
            }

            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(viewModel.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Name = viewModel.Name;
                user.Email = viewModel.Email;
                user.Role = viewModel.Role;

                // Update password only if a new one was provided and valid
                if (!string.IsNullOrEmpty(viewModel.NewPassword) && ModelState.IsValid) // Check ModelState again after potential password errors
                {
                   
                    user.PasswordHash = _hasher.HashPassword(user, viewModel.NewPassword);
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(UserManagement));
            }

            // If ModelState is not valid, return the view with validation errors
            return View(viewModel);
        }


        // GET: Admin/UserDetails/{id}
        public async Task<IActionResult> UserDetails(int? id) // Renamed action to UserDetails
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Will look for a view named UserDetails.cshtml by default
        }

        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Returns the DeleteUser.cshtml view
        }

        [HttpPost, ActionName("DeleteUser")] // Or [HttpPost] if your form's asp-action is "DeleteUser"
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserManagement));
        }

    }
}
