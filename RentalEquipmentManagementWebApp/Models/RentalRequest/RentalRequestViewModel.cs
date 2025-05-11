namespace RentalEquipmentManagementWebApp.Models.RentalRequest
{
    public class RentalRequestViewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }

        public DateTime RentalStartDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Search / Filter fields
        public string? StatusFilter { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? EquipmentSearch { get; set; }

        // Customer Name search instead of Description
        public string? CustomerNameSearch { get; set; }
    }
}
