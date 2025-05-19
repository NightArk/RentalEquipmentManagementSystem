using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementWebApp.Models.Admin;
using RentalEquipmentManagementWebApp.Services;
using System.Data;
using System.Text;
using OfficeOpenXml;
using Rotativa.AspNetCore;



namespace RentalEquipmentManagementWebApp.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EquipmentRentalDBContext _context;
        private readonly IAuditService _auditService;
        private readonly INotificationService _notificationService;


        public AdminController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            EquipmentRentalDBContext context,
            IAuditService auditService,
            INotificationService notificationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _auditService = auditService;
            _notificationService = notificationService;
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

                user.Email = model.Email;
            }

            // Update role if it has changed
            if (user.Role != model.Role)
            {
                var currentRoles = await _userManager.GetRolesAsync(identityUser);
                await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);
                await _userManager.AddToRoleAsync(identityUser, model.Role);
                user.Role = model.Role;
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            // Log the user update
            await _auditService.LogActivityAsync("User Update", $"User {user.Email} was updated by administrator", user.Id);

            // Get current user (admin) to notify
            var currentUserEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);

            if (currentUser != null)
            {
                // Notify current admin user
                await _notificationService.CreateNotificationAsync(
                    currentUser.Id,
                    "User Updated",
                    $"You updated user {user.Email}."
                );
            }

            // Notify the edited user
            await _notificationService.CreateNotificationAsync(
                user.Id,
                "Your Profile Was Updated",
                "Your user profile has been updated by an administrator."
            );

            // Reset password if requested
            if (model.ResetPassword)
            {
                var newPassword = "Password@123"; // Default password
                var token = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                var resetResult = await _userManager.ResetPasswordAsync(identityUser, token, newPassword);

                if (resetResult.Succeeded)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(identityUser, newPassword);
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    // Log password reset
                    await _auditService.LogActivityAsync("Password Reset", $"Password for user {user.Email} was reset by administrator", user.Id);

                    // Notify current admin user about password reset
                    if (currentUser != null)
                    {
                        await _notificationService.CreateNotificationAsync(
                            currentUser.Id,
                            "Password Reset",
                            $"You reset the password for user {user.Email}."
                        );
                    }

                    // Notify the user about their password reset
                    await _notificationService.CreateNotificationAsync(
                        user.Id,
                        "Your Password Was Reset",
                        $"Your password has been reset by an administrator. The new password is: {newPassword}"
                    );

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

            // Notify current admin user about deletion
            var currentUserEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
            if (currentUser != null)
            {
                await _notificationService.CreateNotificationAsync(
                    currentUser.Id,
                    "User Deleted",
                    $"You deleted user {user.Email}."
                );
            }

            TempData["StatusMessage"] = "User deleted successfully.";
            return RedirectToAction(nameof(Users));
        }



        // Export actions for Logs
        [HttpGet]
        public async Task<IActionResult> ExportLogsAsExcel(DateTime? startDate, DateTime? endDate, string? actionType, int? userId)
        {
            var logs = await _auditService.GetLogsAsync(startDate, endDate, actionType, userId);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Logs");

                // Style the header
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Action";
                worksheet.Cells[1, 3].Value = "User";
                worksheet.Cells[1, 4].Value = "Details";
                worksheet.Cells[1, 5].Value = "Source";
                worksheet.Cells[1, 6].Value = "Timestamp";

                // Data
                int row = 2;
                foreach (var log in logs)
                {
                    worksheet.Cells[row, 1].Value = log.Id;
                    worksheet.Cells[row, 2].Value = log.Action;
                    worksheet.Cells[row, 3].Value = log.User?.Name ?? "System";
                    worksheet.Cells[row, 4].Value = log.AffectedData;
                    worksheet.Cells[row, 5].Value = log.Source;

                    if (log.Timestamp.HasValue)
                    {
                        worksheet.Cells[row, 6].Value = log.Timestamp.Value;
                        worksheet.Cells[row, 6].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    }

                    row++;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"Audit_Logs_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportLogsAsCSV(DateTime? startDate, DateTime? endDate, string? actionType, int? userId)
        {
            var logs = await _auditService.GetLogsAsync(startDate, endDate, actionType, userId);
            var builder = new StringBuilder();

            // Add BOM for Excel to recognize UTF-8
            var preamble = Encoding.UTF8.GetPreamble();
            var byteArray = new List<byte>(preamble);

            // Headers
            builder.AppendLine("ID,Action,User,Details,Source,Timestamp");

            // Data
            foreach (var log in logs)
            {
                string detailsEscaped = EscapeCsvField(log.AffectedData ?? "");
                string userNameEscaped = EscapeCsvField(log.User?.Name ?? "System");
                string sourceEscaped = EscapeCsvField(log.Source ?? "");
                // Use ISO 8601 format with 'T' separator
                string timestamp = log.Timestamp?.ToString("yyyy-MM-ddTHH:mm:ss") ?? ""; // 

                builder.AppendLine($"{log.Id},{EscapeCsvField(log.Action)},{userNameEscaped},{detailsEscaped},{sourceEscaped},{timestamp}");
            }

            byteArray.AddRange(Encoding.UTF8.GetBytes(builder.ToString()));

            string fileName = $"Audit_Logs_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            return File(byteArray.ToArray(), "text/csv", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> ExportLogsAsPDF(DateTime? startDate, DateTime? endDate, string? actionType, int? userId)
        {
            var logs = await _auditService.GetLogsAsync(startDate, endDate, actionType, userId);

            string fileName = $"Audit_Logs_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return new ViewAsPdf("ExportLogs", logs)
            {
                FileName = fileName,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 10, 10, 10),
                CustomSwitches = "--disable-smart-shrinking"
            };
        }

        // Export actions for Users
        [HttpGet]
        public async Task<IActionResult> ExportUsersAsExcel()
        {
            var users = await _context.Users.ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Users");

                // Style the header
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Email";
                worksheet.Cells[1, 4].Value = "Role";
                worksheet.Cells[1, 5].Value = "Created At";

                // Data
                int row = 2;
                foreach (var user in users)
                {
                    worksheet.Cells[row, 1].Value = user.Id;
                    worksheet.Cells[row, 2].Value = user.Name;
                    worksheet.Cells[row, 3].Value = user.Email;
                    worksheet.Cells[row, 4].Value = user.Role;

                    if (user.CreatedAt.HasValue)
                    {
                        worksheet.Cells[row, 5].Value = user.CreatedAt.Value;
                        worksheet.Cells[row, 5].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    }

                    row++;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"Users_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportUsersAsCSV()
        {
            var users = await _context.Users.ToListAsync();
            var builder = new StringBuilder();

            // Add BOM for Excel to recognize UTF-8
            var preamble = Encoding.UTF8.GetPreamble();
            var byteArray = new List<byte>(preamble);

            // Headers
            builder.AppendLine("ID,Name,Email,Role,Created At");

            // Data
            foreach (var user in users)
            {
                string nameEscaped = EscapeCsvField(user.Name ?? "");
                string emailEscaped = EscapeCsvField(user.Email ?? "");
                string roleEscaped = EscapeCsvField(user.Role ?? "");
                // Use ISO 8601 format with 'T' separator
                string createdAt = user.CreatedAt?.ToString("yyyy-MM-ddTHH:mm:ss") ?? ""; // 

                builder.AppendLine($"{user.Id},{nameEscaped},{emailEscaped},{roleEscaped},{createdAt}");
            }

            byteArray.AddRange(Encoding.UTF8.GetBytes(builder.ToString()));

            string fileName = $"Users_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            return File(byteArray.ToArray(), "text/csv", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> ExportUsersAsPDF()
        {
            var users = await _context.Users.ToListAsync();

            string fileName = $"Users_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return new ViewAsPdf("ExportUsers", users)
            {
                FileName = fileName,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 10, 10, 10),
                CustomSwitches = "--disable-smart-shrinking"
            };
        }

        // Helper method to properly escape CSV fields
        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            bool requiresQuotes = field.Contains(',') || field.Contains('"') || field.Contains('\r') || field.Contains('\n');

            if (!requiresQuotes)
                return field;

            // Replace double quotes with double double quotes
            field = field.Replace("\"", "\"\"");

            // Wrap in quotes
            return $"\"{field}\"";
        }


  

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(model);
            }

            // Check if email is already in use
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email is already in use");
                ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(model);
            }

            // Create the identity user
            var identityUser = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(model);
            }

            // Add user to role
            await _userManager.AddToRoleAsync(identityUser, model.Role);

            // Create user in custom user table
            var user = new RentalEquipmentManagementLogic.Models.User
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PasswordHash = _userManager.PasswordHasher.HashPassword(identityUser, model.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Log the user creation
            await _auditService.LogActivityAsync("User Created", $"User {user.Email} was created by administrator", user.Id);

            // Notify current user (admin) who created the new user
            var currentUserEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
            if (currentUser != null)
            {
                await _notificationService.CreateNotificationAsync(
                    currentUser.Id,
                    "User Created",
                    $"You successfully created user {user.Email}."
                );
            }

            // Notify new user that their account has been created
            await _notificationService.CreateNotificationAsync(
                user.Id,
                "Account Created",
                "Your user account has been created successfully."
            );

            TempData["StatusMessage"] = "User created successfully.";
            return RedirectToAction(nameof(Users));
        }


    }
}
