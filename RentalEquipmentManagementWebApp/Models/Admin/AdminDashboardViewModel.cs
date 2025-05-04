namespace RentalEquipmentManagementWebApp.Models.Admin
{
    public class AdminDashboardViewModel
    {
        public Dictionary<string, int> RequestStatusCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> EquipmentStatusCounts { get; set; } = new Dictionary<string, int>();
        public List<CategoryStatViewModel> CategoryDistribution { get; set; } = new List<CategoryStatViewModel>();
        public List<RecentTransactionViewModel> RecentTransactions { get; set; } = new List<RecentTransactionViewModel>();
        public FinancialSummaryViewModel FinancialSummary { get; set; } = new FinancialSummaryViewModel();
    }

    public class CategoryStatViewModel
    {
        public string CategoryName { get; set; } = string.Empty;
        public int EquipmentCount { get; set; }
    }

    public class RecentTransactionViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string EquipmentName { get; set; } = string.Empty;
        public DateTime? RentalDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }

    public class FinancialSummaryViewModel
    {
        public decimal TotalRevenue { get; set; }
        public decimal PendingPayments { get; set; }
        public decimal AdditionalCharges { get; set; }
        public decimal NetRevenue => TotalRevenue - PendingPayments;
    }
}
