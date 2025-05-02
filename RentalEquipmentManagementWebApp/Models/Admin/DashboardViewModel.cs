using RentalEquipmentManagementLogic.Models;

namespace RentalEquipmentManagementWebApp.Models.Admin
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalEquipment { get; set; }
        public int TotalCategories { get; set; }
        public int TotalRentalRequests { get; set; }
        public int TotalRentalTransactions { get; set; }
        public int TotalReturnRecords { get; set; }
        public IEnumerable<Log> RecentLogs { get; set; } = new List<Log>();
    }
}
