using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic.Models;

namespace RentalEquipmentManagementWebApp.Services
{
    public class AuditService : IAuditService
    {
        private readonly EquipmentRentalDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(EquipmentRentalDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogActivityAsync(string action, string details, int? userId = null)
        {
            // If userId is not provided, try to get it from the current user
            if (userId == null && _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int id))
                {
                    userId = id;
                }
            }

            var log = new Log
            {
                Action = action,
                AffectedData = details,
                UserId = userId,
                Source = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.Now
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Log>> GetLogsAsync(DateTime? startDate = null, DateTime? endDate = null, string? actionType = null, int? userId = null)
        {
            IQueryable<Log> query = _context.Logs.Include(l => l.User);

            if (startDate.HasValue)
            {
                query = query.Where(l => l.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(l => l.Timestamp <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(actionType))
            {
                query = query.Where(l => l.Action == actionType);
            }

            if (userId.HasValue)
            {
                query = query.Where(l => l.UserId == userId.Value);
            }

            return await query.OrderByDescending(l => l.Timestamp).ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetRecentLogsAsync(int count = 50)
        {
            return await _context.Logs
                .Include(l => l.User)
                .OrderByDescending(l => l.Timestamp)
                .Take(count)
                .ToListAsync();
        }
    }
}
