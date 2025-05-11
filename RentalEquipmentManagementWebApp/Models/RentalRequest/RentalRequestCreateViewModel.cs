using Microsoft.AspNetCore.Mvc.Rendering;

namespace RentalEquipmentManagementWebApp.Models.RentalRequest
{
    public class RentalRequestCreateViewModel
    {
        public int UserId { get; set; }
        public int EquipmentId { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Description { get; set; }

        // Equipment dropdown list for selection
        public List<SelectListItem> EquipmentList { get; set; }

        // User dropdown list for Admin/Manager role selection
        public List<SelectListItem> UserList { get; set; }
    }
}
