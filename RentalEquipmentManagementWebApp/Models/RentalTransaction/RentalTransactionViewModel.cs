namespace RentalEquipmentManagementWebApp.Models.RentalTransaction
{
    public class RentalTransactionViewModel
    {
        public int Id { get; set; }
        public int RentalRequestId { get; set; }
        public int AssignedEquipmentId { get; set; }
        public string CustomerName { get; set; }
        public string EquipmentName { get; set; }
        public DateTime ActualRentalStartDate { get; set; }  // Nullable DateTime
        public DateTime ReturnDate { get; set; }  // Nullable DateTime
        public int RentalPeriod { get; set; }
        public decimal RentalFee { get; set; }
        public decimal Deposit { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }  // Nullable DateTime

        // List of documents for the rental transaction
        public List<DocumentViewModel> Documents { get; set; }

        // Add this to handle file uploads
        public List<IFormFile> Files { get; set; }
    }

    public class DocumentViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }  // The name of the file
        public string FileType { get; set; }  // The file type (e.g., pdf, jpg, etc.)

        // The file content stored as a byte array (for server-side management)
        public byte[] FileData { get; set; }

        public DateTime UploadedAt { get; set; }
    }

}
