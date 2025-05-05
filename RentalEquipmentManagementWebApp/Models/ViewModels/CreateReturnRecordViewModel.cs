using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RentalEquipmentManagementWebApp.Models.ViewModels
{
    public class CreateReturnRecordViewModel
    {
        [Required(ErrorMessage = "Please select a Rental Transaction.")]
        public int? RentalTransactionId { get; set; }

        [Required(ErrorMessage = "Customer Name is required.")] // Although read-only, we require a transaction to be selected
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Equipment Name is required.")] // Although read-only, we require a transaction to be selected
        public string? EquipmentName { get; set; }

        [Required(ErrorMessage = "Please enter the Actual Return Date.")]
        public DateTime? ActualReturnDate { get; set; }

        [Required(ErrorMessage = "Please select the Return Condition.")]
        public string? ReturnCondition { get; set; }

        [Required(ErrorMessage = "Late Return Fee is required.")] // Making this required as per your request
        [Range(0, double.MaxValue, ErrorMessage = "Late Return Fee must be a non-negative value.")]
        public decimal? LateReturnFee { get; set; }

        [Required(ErrorMessage = "Additional Charges are required.")] // Making this required as per your request
        [Range(0, double.MaxValue, ErrorMessage = "Additional Charges must be a non-negative value.")]
        public decimal? AdditionalCharges { get; set; }

        public List<SelectListItem>? RentalTransactions { get; set; }

        public List<SelectListItem> ReturnConditions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Select Condition --" },
            new SelectListItem { Value = "Good", Text = "Good" },
            new SelectListItem { Value = "Damaged", Text = "Damaged" },
            new SelectListItem { Value = "Lost", Text = "Lost" }
        };
    }
}
