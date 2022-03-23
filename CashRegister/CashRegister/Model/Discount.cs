using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class Discount
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Category Category { get; set; }
    }
}
