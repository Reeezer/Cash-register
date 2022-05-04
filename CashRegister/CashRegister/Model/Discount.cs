using System;

namespace CashRegister.Model
{
    public class Discount
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually. It is handled by the DB.
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// Start date of the discount
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// End date of the discount
        /// </summary>
        public DateTime EndDate { get; set;  }
        /// <summary>
        /// Category of the discount concerned by the discount
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// Percentage of the discount
        /// </summary>
        public int Percentage { get; set; }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        /// <param name="startDate">The start date of the discount</param>
        /// <param name="endDate">The end date of the discount</param>
        /// <param name="category">The category concerned of the discount</param>
        /// <param name="percentage">The percentage of the discount</param>
        public Discount(DateTime startDate, DateTime endDate, Category category, int percentage)
        {
            StartDate = startDate;
            EndDate = endDate;
            Category = category;
            Percentage = percentage;
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Discount()
        {
        }
    }
}
