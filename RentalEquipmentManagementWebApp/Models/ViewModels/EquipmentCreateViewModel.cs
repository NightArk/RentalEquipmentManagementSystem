using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // For validation attributes
using RentalEquipmentManagementLogic.Models; // Replace with your actual namespace

namespace RentalEquipmentManagementWebApp.Models.ViewModels
{
    public class EquipmentCreateViewModel
    {
        public int Id { get; set; } // Add the Id property


        [Required(ErrorMessage = "Equipment Name is required")]
        [StringLength(100, ErrorMessage = "Equipment Name cannot exceed 100 characters")]
        [Display(Name = "Equipment Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Rental Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rental Price must be greater than 0")]
        [Display(Name = "Rental Price")]
        public decimal RentalPrice { get; set; }

        [Required(ErrorMessage = "Availability Status is required")]
        [Display(Name = "Availability Status")]
        public string AvailabilityStatus { get; set; }

        [Required(ErrorMessage = "Condition Status is required")]
        [Display(Name = "Condition Status")]
        public string ConditionStatus { get; set; }

        public List<Category> Categories { get; set; } // For the Category dropdown
    }
}