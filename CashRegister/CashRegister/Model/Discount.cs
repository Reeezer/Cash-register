using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class Discount
    {
        private static int id = 1;

        public int ID { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public Category Category { get; }
        public int Percentage { get; }

        public Discount(DateTime startDate, DateTime endDate, Category category, int percentage)
        {
            ID = id++;
            StartDate = startDate;
            EndDate = endDate;
            Category = category;
            Percentage = percentage;
        }
    }
}
