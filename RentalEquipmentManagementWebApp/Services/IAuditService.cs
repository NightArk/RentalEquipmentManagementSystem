using RentalEquipmentManagementLogic.Models;

namespace RentalEquipmentManagementWebApp.Services
{
    public interface IAuditService
    {
        Task LogActivityAsync(string action, string details, int? userId = null);
        Task<IEnumerable<Log>> GetLogsAsync(DateTime? startDate = null, DateTime? endDate = null, string? actionType = null, int? userId = null);
        Task<IEnumerable<Log>> GetRecentLogsAsync(int count = 50);
    }
}
