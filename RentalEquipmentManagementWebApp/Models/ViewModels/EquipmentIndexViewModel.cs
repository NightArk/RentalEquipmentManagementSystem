using System.Collections.Generic;
using RentalEquipmentManagementLogic.Models; // Replace with your actual namespace

namespace RentalEquipmentManagementWebApp.Models.ViewModels
{
    public class EquipmentIndexViewModel
    {
        public List<Equipment> Equipment { get; set; }
        public List<Category> Categories { get; set; }
        public string SearchString { get; set; }
        public string AvailabilityFilter { get; set; }
        public string ConditionFilter { get; set; }
        public int? CategoryFilter { get; set; }
    }
}