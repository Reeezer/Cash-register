using System;
using SQLite;

namespace CashRegister.Model
{
    public class Item : IComparable
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public Category Category { get; }
        public string Name { get; }
        public double Price { get; }
        public int Quantity { get; set; }
        public string EAN { get; }

        public Item() 
        {

        }

        public Item(Category category, string v1, double v2, int v3, string v4)
        {
            Category = category;
            Name = v1;
            Price = v2;
            Quantity = v3;
            EAN = v4;
        }

        public int CompareTo(object obj)
        {
            Item i2 = obj as Item;
            return Id.CompareTo(i2.Id);
        }
    }
}
