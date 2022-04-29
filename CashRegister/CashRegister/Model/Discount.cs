using System;

namespace CashRegister.Model
{
    public class Discount
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually
        /// </summary>
        public int Id { get; set; } = 0;

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
