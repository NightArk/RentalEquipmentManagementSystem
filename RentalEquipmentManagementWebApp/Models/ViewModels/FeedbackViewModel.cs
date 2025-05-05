namespace RentalEquipmentManagementWebApp.Models.ViewModels
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }
        public int RentalTransactionId { get; set; }
        public string CustomerName { get; set; }
        public string EquipmentName { get; set; }
        public string Comment { get; set; }
        public int? Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsHidden { get; internal set; }
    }
}
