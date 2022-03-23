using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CashRegister.Model
{
    public class Item
    {
        public int ID { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int EAN { get; set; }
    }
}
