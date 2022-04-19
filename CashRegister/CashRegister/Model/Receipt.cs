using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CashRegister.Model
{
    public class Receipt
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; }
        public User Client { get; set; }
        public Discount Discount { get; set; }

        public Receipt()
        {
            Date = DateTime.Now;
        }
    }
}
