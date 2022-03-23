using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class ReceiptLine
    {
        public static int id = 1;

        public int ID { get; set; }
        public Receipt Receipt { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }

        public ReceiptLine(Receipt receipt, Item item, int quantity = 1)
        {
            ID = id++;
            Receipt = receipt;
            Item = item;
            Quantity = quantity;
        }
    }
}
