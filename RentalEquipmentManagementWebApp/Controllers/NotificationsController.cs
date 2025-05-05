using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;
using System.Security.Claims;

namespace RentalEquipmentManagementWebApp.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly EquipmentRentalDBContext _context; 

        public NotificationsController(EquipmentRentalDBContext context)
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

            var currentUserId = GetCurrentUserIdAsInt();
            if (!currentUserId.HasValue || notification.UserId != currentUserId.Value)
            {
                return Unauthorized();
            }

            // Mark the notification as read when viewing details
            if (notification.Status == "Unread")
            {
                notification.Status = "Read";
                await _context.SaveChangesAsync();
            }

            return View(notification); // Create a Details.cshtml view
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

