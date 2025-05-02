using System.ComponentModel.DataAnnotations;

namespace RentalEquipmentManagementWebApp.Models.Equipment
{
    public class EquipmentCreateViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Equipment Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [Display(Name = "Rental Price")]
        public decimal RentalPrice { get; set; }
    }
}
