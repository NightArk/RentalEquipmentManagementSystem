using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.Account;
using RentalEquipmentManagementWebApp.Services;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EquipmentRentalDBContext _context;
        private readonly IAuditService _auditService;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            EquipmentRentalDBContext context,
            IAuditService auditService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _auditService = auditService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true // For simplicity, we're auto-confirming emails
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Add user to Customer role
                    await _userManager.AddToRoleAsync(user, "Customer");

                    // Create corresponding user in our custom User table
                    var customUser = new User
                    {
                        Name = model.Name,
                        Email = model.Email,
                        PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password),
                        Role = "Customer",
                        CreatedAt = DateTime.Now
                    };

                    _context.Users.Add(customUser);
                    await _context.SaveChangesAsync();

                    // Log the registration
                    await _auditService.LogActivityAsync("User Registration", $"User {model.Email} registered successfully", customUser.Id);

                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    // Get the identity user
                    var identityUser = await _userManager.FindByEmailAsync(model.Email);
                    
                    // Get the custom user
                    var customUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                    
                    if (customUser != null)
                    {
                        // Log the login
                        await _auditService.LogActivityAsync("User Login", $"User {model.Email} logged in successfully", customUser.Id);
                    }

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                
                if (result.IsLockedOut)
                {
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Get the current user before signing out
            var user = await _userManager.GetUserAsync(User);
            
            await _signInManager.SignOutAsync();
            
            if (user != null)
            {
                var customUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                if (customUser != null)
                {
                    // Log the logout
                    await _auditService.LogActivityAsync("User Logout", $"User {user.Email} logged out", customUser.Id);
                }
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var customUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (customUser == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                Name = customUser.Name,
                Email = customUser.Email
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var customUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (customUser == null)
            {
                return NotFound();
            }

            // Update custom user
            customUser.Name = model.Name;
            
            // Only update email if it has changed
            if (customUser.Email != model.Email)
            {
                // Update identity user email
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                var setUserNameResult = await _userManager.SetUserNameAsync(user, model.Email);
                if (!setUserNameResult.Succeeded)
                {
                    foreach (var error in setUserNameResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                // Update custom user email
                customUser.Email = model.Email;
            }

            // Save changes to custom user
            _context.Update(customUser);
            await _context.SaveChangesAsync();

            // Log the profile update
            await _auditService.LogActivityAsync("Profile Update", $"User {customUser.Email} updated their profile", customUser.Id);

            // If password is provided, update it
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                // Update custom user password hash
                customUser.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                _context.Update(customUser);
                await _context.SaveChangesAsync();

                // Log the password change
                await _auditService.LogActivityAsync("Password Change", $"User {customUser.Email} changed their password", customUser.Id);

                // Re-sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            TempData["StatusMessage"] = "Your profile has been updated";
            return RedirectToAction(nameof(Profile));
        }
    }
}
