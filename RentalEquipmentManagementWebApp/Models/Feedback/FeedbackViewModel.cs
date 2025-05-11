namespace RentalEquipmentManagementWebApp.Models.Feedback
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EquipmentId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsHide { get; set; }

        // Optional: Include Navigation Properties or other related information if needed
        public string UserName { get; set; } // This could be from the Users table
        public string EquipmentName { get; set; } // This could be from the Equipment table
    }
}
