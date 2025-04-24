using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalEquipmentManagementApp
{
    public class RentalRequestGridItem
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Equipment { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public string CreatedAt { get; set; }
    }
}
