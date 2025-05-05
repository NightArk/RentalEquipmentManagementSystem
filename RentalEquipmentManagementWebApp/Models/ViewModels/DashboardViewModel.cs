namespace RentalEquipmentManagementWebApp.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalActiveRentals { get; set; }
        public int TotalAvailableEquipment { get; set; }
        public int TotalEquipment { get; set; }
        public int NewPendingRequests { get; set; }
        public List<EquipmentAvailabilityStatus> EquipmentAvailability { get; set; }
        public decimal TotalRevenueLastMonth { get; set; }
        public List<CategoryPendingRequests> PendingRequestsByCategory { get; set; }
        public List<DamagedEquipment> DamagedEquipmentReport { get; set; }
        public List<OverdueRental> OverdueRentals { get; set; }
        public double AverageFeedbackRating { get; set; }
        // Add more properties as needed for your dashboard

        public DashboardViewModel()
        {
            EquipmentAvailability = new List<EquipmentAvailabilityStatus>();
            PendingRequestsByCategory = new List<CategoryPendingRequests>();
            DamagedEquipmentReport = new List<DamagedEquipment>();
            OverdueRentals = new List<OverdueRental>();
        }
    }

    public class EquipmentAvailabilityStatus
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class CategoryPendingRequests
    {
        public string CategoryName { get; set; }
        public int PendingRequestsCount { get; set; }
    }

    public class DamagedEquipment
    {
        public string EquipmentName { get; set; }
        public string ConditionStatus { get; set; }
    }

    public class OverdueRental
    {
        public int TransactionId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
