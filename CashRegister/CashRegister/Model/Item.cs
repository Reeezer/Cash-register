using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CashRegister.Model
{
    public class Item
    {
        public static int id = 1;

        public int ID { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int EAN { get; set; }

        public Item(Category category, string name, double price, int quantity, int ean)
        {
            ID = id++;
            Category = category;
            Name = name;
            Price = price;
            Quantity = quantity;
            EAN = ean;
        }
    }
}
