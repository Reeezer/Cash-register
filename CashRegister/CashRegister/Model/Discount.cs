using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CashRegister.Model
{
    public class Discount
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set;  }
        public Category Category { get; set; }
        public int Percentage { get; set; }

        public Discount(DateTime startDate, DateTime endDate, Category category, int percentage)
        {
            StartDate = startDate;
            EndDate = endDate;
            Category = category;
            Percentage = percentage;
        }

        public Discount()
        {
        }
    }
}
