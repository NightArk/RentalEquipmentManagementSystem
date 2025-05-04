using RentalEquipmentManagementLogic.Models;
using Equipment1 = RentalEquipmentManagementLogic.Models.Equipment;
namespace RentalEquipmentManagementWebApp.Models.Home
{
    public class HomeViewModel
    {
        public int CategoryCount { get; set; }
        public int EquipmentCount { get; set; }
        public int AvailableEquipmentCount { get; set; }
        public IEnumerable<Equipment1> FeaturedEquipment { get; set; } = new List<Equipment1>();
    }
}
