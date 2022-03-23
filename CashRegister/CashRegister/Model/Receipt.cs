using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class Receipt
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public User Client { get; set; }
        public Discount Discount { get; set; }
        public IEnumerable<ReceiptLine> Lines { get; set; }
    }
}
