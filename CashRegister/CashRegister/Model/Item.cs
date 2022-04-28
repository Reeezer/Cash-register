using System;
using SQLite;

namespace CashRegister.Model
{
    public class Item : IComparable
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually
        /// </summary>
        public int Id { get; set; } = 0;

        public Category Category { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string EAN { get; set; }

        public Item(Category category, string name, double price, int quantity, string ean)
        {
            Category = category;
            Name = name;
            Price = price;
            Quantity = quantity;
            EAN = ean;
        }

        public Item()
        {
        }

        public int CompareTo(object obj)
        {
            Item i2 = obj as Item;
            return Id.CompareTo(i2.Id);
        }
    }
}
