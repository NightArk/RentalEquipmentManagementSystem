namespace RentalEquipmentManagementWebApp.Models.ViewModels
{
    public class ReturnRecordViewModel
    {
        public int ReturnId { get; set; }
        public int? RentalTransactionId { get; set; }
        public DateTime ActualReturnDate { get; set; }
        public string? ReturnCondition { get; set; }
        public decimal? LateReturnFee { get; set; }
        public decimal? AdditionalCharges { get; set; }

        // Information from related entities that the view might need
        public int? EquipmentId { get; set; }
        public string? EquipmentName { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; } // Example of another useful property
    }
}
