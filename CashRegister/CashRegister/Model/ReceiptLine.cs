using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class ReceiptLine
    {
        public int ID { get; set; }
        public Receipt Receipt { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
