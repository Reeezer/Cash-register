using System;

namespace CashRegister.Model
{
    public class Receipt
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually
        /// </summary>
        public int Id { get; set; } = 0;

        public DateTime Date { get; set; }
        public User Client { get; set; }
        public Discount Discount { get; set; }

        public Receipt()
        {
            Date = DateTime.Now;
        }
    }
}
