using Microsoft.AspNetCore.Mvc;
using RentalEquipmentManagementLogic.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using RentalEquipmentManagementLogic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace RentalEquipmentManagementWebApp.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly EquipmentRentalDBContext _context;
        //private readonly UserDto _currentUser;

        // Constructor to inject the database context
        public AuthenticationController(EquipmentRentalDBContext context)
        {
            _context = context;
            //_currentUser = currentUser;
        }

        // Displays the Login view
        public IActionResult Login()
        {
            ViewData["ActivePage"] = "Login"; // Set active page for navigation
            return View();
        }



        // Handles POST request for login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                TempData["LoginStatus"] = "Error";
                TempData["LoginMessage"] = "Email and password are required.";
                return RedirectToAction("Login");
            }

            if (!IsValidEmail(email))
            {
                TempData["LoginStatus"] = "Error";
                TempData["LoginMessage"] = "Invalid email format.";
                return RedirectToAction("Login");
            }

            if (password.Length < 6)
            {
                TempData["LoginStatus"] = "Error";
                TempData["LoginMessage"] = "Password must be at least 6 characters.";
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                TempData["LoginStatus"] = "Error";
                TempData["LoginMessage"] = "Invalid email or password.";
                return RedirectToAction("Login");
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                TempData["LoginStatus"] = "Error";
                TempData["LoginMessage"] = "Invalid email or password.";
                return RedirectToAction("Login");
            }

            // Create user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Keeps user logged in across sessions
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Log the successful login
            var log = new Log
            {
                UserId = user.Id,
                Action = "Login",
                Timestamp = DateTime.Now,
                AffectedData = "Login successful",
                Source = "Web"

            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            // Redirect based on role
            return user.Role.ToLower() switch
            {
                "admin" => RedirectToAction("Dashboard", "Admin"),
                "manager" => RedirectToAction("Dashboard", "Monitoring"),
                "customer" => RedirectToAction("Index", "RentalRequest"),
                _ => RedirectToAction("Index", "Home")
            };
        }



        // Displays the Register view
        public IActionResult Register()
        {
            ViewData["ActivePage"] = "Register"; // Set active page for navigation
            return View();
        }

        // Handles POST request for registration (async version)
        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password, string confirmPassword)
        {
            // Validate if required fields are provided
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                TempData["RegisterStatus"] = "Error";
                TempData["RegisterMessage"] = "All fields are required.";
                return RedirectToAction("Register");
            }

            // Validate email format
            if (!IsValidEmail(email))
            {
                TempData["RegisterStatus"] = "Error";
                TempData["RegisterMessage"] = "Invalid email format.";
                return RedirectToAction("Register");
            }

            // Validate password length
            if (password.Length < 6)
            {
                TempData["RegisterStatus"] = "Error";
                TempData["RegisterMessage"] = "Password must be at least 6 characters.";
                return RedirectToAction("Register");
            }

            // Check if passwords match
            if (password != confirmPassword)
            {
                TempData["RegisterStatus"] = "Error";
                TempData["RegisterMessage"] = "Passwords do not match.";
                return RedirectToAction("Register");
            }

            // Check if the email already exists in the database (async)
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == email);
            if (emailExists)
            {
                TempData["RegisterStatus"] = "Error";
                TempData["RegisterMessage"] = "An account with this email already exists.";
                return RedirectToAction("Register");
            }

            // Assign the role as 'User' (since the role is hardcoded now)
            string role = "User";  // Default to 'User'

            // Hash the password before saving it
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Name = name,
                Email = email,
                Role = role,  // Set to 'User' by default
                CreatedAt = DateTime.Now,
                PasswordHash = hasher.HashPassword(null, password)
            };

            // Save the new user to the database (async)
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Log registration success (commented out)
            // var log = new Log
            // {
            //     UserId = user.Id,
            //     Action = "Register",
            //     Timestamp = DateTime.Now,
            //     AffectedData = "Registration successful",
            //     Source = "Web"  // Assuming web as the source
            // };
            // _context.Logs.Add(log);
            // await _context.SaveChangesAsync();

            // Set success status and message
            TempData["RegisterStatus"] = "Success";
            TempData["RegisterMessage"] = "Registration successful!";

            // Redirect to the Login page after successful registration
            return RedirectToAction("Login");
        }



        // Handles user logout
        public IActionResult Logout()
        {
            // Sign out the user and clear authentication cookies
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Log logout action (commented out)
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (userId != null)
            // {
            //     var log = new Log
            //     {
            //         UserId = int.Parse(userId),
            //         Action = "Logout",
            //         Timestamp = DateTime.Now,
            //         AffectedData = "User logged out",
            //         Source = "Web"
            //     };
            //     _context.Logs.Add(log);
            //     _context.SaveChanges();
            // }

            // Redirect to the Home page or the desired location after logout
            return RedirectToAction("Index", "Home");
        }



        // Helper method to validate the email format using regex
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
