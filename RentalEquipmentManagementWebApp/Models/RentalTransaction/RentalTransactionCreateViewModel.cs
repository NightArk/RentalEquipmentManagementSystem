using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RentalEquipmentManagementWebApp.Models.RentalTransaction
{
    public class RentalTransactionCreateViewModel
    {
       

        [Required(ErrorMessage = "Rental Request is required.")]
        public int RentalRequestId { get; set; }
       

        [Required(ErrorMessage = "Assigned Equipment ID is required.")]
        public int AssignedEquipmentId { get; set; }
        

        // Add CustomerId
        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerId { get; set; }
        

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime ActualRentalStartDate { get; set; }

        [Required(ErrorMessage = "Return Date is required.")]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Rental Period is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Rental Period must be a positive number.")]
        public int RentalPeriod { get; set; }

        [Required(ErrorMessage = "Rental Fee is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Rental Fee must be a positive number.")]
        public decimal RentalFee { get; set; }

        [Required(ErrorMessage = "Deposit is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Deposit must be a positive number.")]
        public decimal Deposit { get; set; }

        [Required(ErrorMessage = "Payment Status is required.")]
        public string PaymentStatus { get; set; }

        // This property is for uploading documents (e.g., rental agreements, receipts)
        public List<IFormFile> Files { get; set; }
    }
}
