using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalEquipmentManagementApp
{
    public class RentalRecordGridItem
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Equipment { get; set; }
        public DateTime ActualReturnDate { get; set; }
        public string ReturnCondition { get; set; }
        public decimal LateReturnFee { get; set; }
        public decimal AdditionalCharges { get; set; }
        public string CreatedAt { get; set; }
    }
}
