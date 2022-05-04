using System;

namespace CashRegister.Model
{
    public class Receipt
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually. It is handled by the DB.
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// Date of the creation of the receipt
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// The client liked to the recepit
        /// </summary>
        public User Client { get; set; }
        /// <summary>
        /// The discount present on the receipt.
        /// </summary>
        public Discount Discount { get; set; }

        /// <summary>
        /// Constructor of the receipt.
        /// </summary>
        public Receipt()
        {
            Date = DateTime.Now;
        }
    }
}
