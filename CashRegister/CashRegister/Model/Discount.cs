using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class Discount
    {
        public static int id = 1;

        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Category Category { get; set; }

        public Discount(DateTime startDate, DateTime endDate, Category category)
        {
            ID = id++;
            StartDate = startDate;
            EndDate = endDate;
            Category = category;
        }
    }
}
