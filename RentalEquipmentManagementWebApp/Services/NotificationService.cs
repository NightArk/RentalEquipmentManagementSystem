using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;

namespace RentalEquipmentManagementWebApp.Services
{
    public class NotificationService : INotificationService
    {
        private readonly EquipmentRentalDBContext _context;

        public NotificationService(EquipmentRentalDBContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(int userId, string title, string message, string? relatedEntityType = null, int? relatedEntityId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                NotificationType = relatedEntityType ?? "System",
                MessageContent = $"{title}: {message}",
                Status = "Unread",
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false)
        {
            IQueryable<Notification> query = _context.Notifications.Where(n => n.UserId == userId);

            if (unreadOnly)
            {
                query = query.Where(n => n.Status == "Unread");
            }

            return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.Status = "Read";
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && n.Status == "Unread")
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.Status = "Read";
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && n.Status == "Unread");
        }
    }
}
