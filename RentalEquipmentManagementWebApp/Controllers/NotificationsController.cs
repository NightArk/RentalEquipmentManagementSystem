using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace RentalEquipmentManagementWebApp.Controllers
{

    [Authorize(Policy = "RequireAuthenticated")]
    public class NotificationsController : Controller
    {
        private readonly EquipmentRentalDBContext _context;
        

        public NotificationsController(EquipmentRentalDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = await GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var notifications = await _context.Notifications
                .Where(n => n.UserId == currentUserId.Value)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notifications);
        }

        public async Task<IActionResult> Details(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            var currentUserId = await GetCurrentUserId();
            if (currentUserId == null || notification.UserId != currentUserId.Value)
            {
                return Unauthorized();
            }

            // Mark the notification as read when viewing details
            if (notification.Status == "Unread")
            {
                notification.Status = "Read";
                await _context.SaveChangesAsync();
            }

            return View(notification); // Ensure you have a Details.cshtml view
        }

        private async Task<int?> GetCurrentUserId()
        {
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            return currentUser?.Id;
        }
    }
}
