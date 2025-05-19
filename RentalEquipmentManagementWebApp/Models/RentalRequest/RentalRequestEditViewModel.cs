using System.ComponentModel.DataAnnotations;

namespace RentalEquipmentManagementWebApp.Models.RentalRequest
{
    public class RentalRequestEditViewModel
    {
        public int Id { get; set; }

        // Always displayed, but sometimes read-only
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }

        public decimal TotalCost { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        [MaxLength(50)]
        public string Status { get; set; }

        // To control what is editable in the Razor view
        public bool CanEditDates { get; set; }
        public bool CanEditDescription { get; set; }
    }
}
