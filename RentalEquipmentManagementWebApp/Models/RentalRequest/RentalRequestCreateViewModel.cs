using System;
using System.ComponentModel.DataAnnotations;

namespace RentalEquipmentManagementWebApp.Models.RentalRequest
{
    public class RentalRequestCreateViewModel
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Equipment ID is required.")]
        [Display(Name = "Equipment")]
        public int EquipmentId { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Return Date is required.")]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Total Cost is required.")]
        [Display(Name = "Total Cost")]
        [Range(0, double.MaxValue, ErrorMessage = "Total Cost must be greater than or equal to 0.")]
        public decimal TotalCost { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Description")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }


    }
}
