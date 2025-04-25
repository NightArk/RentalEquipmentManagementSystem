using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalEquipmentManagementApp
{
    public class RentalTransactionRow
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Equipment { get; set; }
        public DateTime ActualRentalStartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int RentalPeriod { get; set; }
        public decimal RentalFee { get; set; }
        public decimal Deposit { get; set; }
        public string PaymentStatus { get; set; }
        public string CreatedAt { get; set; }
    }

}
