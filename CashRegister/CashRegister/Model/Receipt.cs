using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class Receipt
    {
        public static int id = 1;

        public int ID { get; set; }
        public DateTime Date { get; set; }
        public User Client { get; set; }
        public Discount Discount { get; set; }
        public IEnumerable<ReceiptLine> Lines { get; set; }

        public Receipt(DateTime date, User client)
        {
            ID = id++;
            Date = date;
            Client = client;
            Lines = new List<ReceiptLine>();
        }
    }
}
