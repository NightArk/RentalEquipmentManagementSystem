
namespace RentalEquipmentManagementWebApp.Models.ViewModels
{
    public class RentalRequestIndexViewModel
    {
        public List<RentalRequestListItemViewModel> RentalRequests { get; set; }
        public string SearchString { get; set; }
        public string StatusFilter { get; set; }
        public List<string> StatusList { get; set; }
    }

    public class RentalRequestListItemViewModel
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string EquipmentName { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public string? Status { get; set; }
        public string? CustomerName { get; set; }
        public string? Description { get; set; } 
    }
}
