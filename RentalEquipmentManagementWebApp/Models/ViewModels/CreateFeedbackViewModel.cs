using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RentalEquipmentManagementWebApp.Models.ViewModels
{
    public class CreateFeedbackViewModel
    {
        public string EquipmentName { get; set; }

        public int EquipmentId { get; set; }

        [Required(ErrorMessage = "Please enter your feedback comment.")]
        [StringLength(500, ErrorMessage = "The comment cannot exceed {1} characters.")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Please provide a rating.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }
    }
}
