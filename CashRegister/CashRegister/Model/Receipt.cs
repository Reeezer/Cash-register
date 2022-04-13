using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class Receipt
    {
        private static int id = 1;

        public int ID { get; }
        public DateTime Date { get; }
        public User Client { get; set; }
        public Discount Discount { get; set; }

        public Receipt()
        {
            ID = id++;
            Date = DateTime.Now;
        }

        public bool Contains(Item item)
        {
            return true; // FIXME
        }
    }
}
