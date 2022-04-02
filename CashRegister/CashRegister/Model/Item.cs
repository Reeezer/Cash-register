using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CashRegister.Model
{
    public class Item
    {
        public static int id = 1;

        public int ID { get; }
        public Category Category { get; }
        public string Name { get; }
        public double Price { get; }
        public int Quantity { get; set; }
        public string EAN { get; }

        public Item(Category category, string name, double price, int quantity, string ean)
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
