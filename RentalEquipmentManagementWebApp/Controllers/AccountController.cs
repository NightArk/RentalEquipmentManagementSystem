using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentalEquipmentManagementLogic;
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
        private readonly ISharedAuthenticationService _sharedAuthService;

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
            _sharedAuthService = new SharedAuthenticationService(context);
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
                        Role = "Customer",
                        CreatedAt = DateTime.Now
                    };

                    // Use shared service to hash password
                    customUser.PasswordHash = await _sharedAuthService.HashPasswordAsync(model.Password, customUser);

                    _context.Users.Add(customUser);
                    await _context.SaveChangesAsync();

                    // Log the registration
                    await _sharedAuthService.LogUserActivityAsync(customUser.Id, "User Registration",
                        $"User {model.Email} registered successfully", "Web");

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
                // Authenticate using Identity first
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    // Also validate password using shared service
                    var isValidPassword = await _sharedAuthService.ValidatePasswordAsync(model.Email, model.Password);

                    if (!isValidPassword)
                    {
                        // Log the discrepancy but still allow login
                        var user = await _sharedAuthService.GetUserByEmailAsync(model.Email);
                        if (user != null)
                        {
                            await _sharedAuthService.LogUserActivityAsync(user.Id, "Password Validation Discrepancy",
                                "Identity validation succeeded but shared validation failed", "Web");

                            // Update the custom user's password hash to match Identity's hash
                            var identityUser = await _userManager.FindByEmailAsync(model.Email);
                            user.PasswordHash = await _sharedAuthService.HashPasswordAsync(model.Password, user);
                            _context.Update(user);
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Get the custom user
                    var customUser = await _sharedAuthService.GetUserByEmailAsync(model.Email);

                    if (customUser != null)
                    {
                        // Log the login
                        await _sharedAuthService.LogUserActivityAsync(customUser.Id, "User Login",
                            $"User {model.Email} logged in successfully", "Web");
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
                var customUser = await _sharedAuthService.GetUserByEmailAsync(user.Email);
                if (customUser != null)
                {
                    // Log the logout
                    await _sharedAuthService.LogUserActivityAsync(customUser.Id, "User Logout",
                        $"User {user.Email} logged out", "Web");
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

            var customUser = await _sharedAuthService.GetUserByEmailAsync(user.Email);
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

            var customUser = await _sharedAuthService.GetUserByEmailAsync(user.Email);
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
            await _sharedAuthService.LogUserActivityAsync(customUser.Id, "Profile Update",
                $"User {customUser.Email} updated their profile", "Web");

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

                // Update custom user password hash using shared service
                customUser.PasswordHash = await _sharedAuthService.HashPasswordAsync(model.NewPassword, customUser);
                _context.Update(customUser);
                await _context.SaveChangesAsync();

                // Log the password change
                await _sharedAuthService.LogUserActivityAsync(customUser.Id, "Password Change",
                    $"User {customUser.Email} changed their password", "Web");

                // Re-sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            TempData["StatusMessage"] = "Your profile has been updated";
            return RedirectToAction(nameof(Profile));
        }
    }
}