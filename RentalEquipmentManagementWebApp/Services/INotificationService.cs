using RentalEquipmentManagementLogic.Models;

namespace RentalEquipmentManagementWebApp.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(int userId, string title, string message, string? relatedEntityType = null, int? relatedEntityId = null);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
    }
}
