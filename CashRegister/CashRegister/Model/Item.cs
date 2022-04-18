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

        public Item() { }


        public int CompareTo(object obj)
        {
            Item i2 = obj as Item;
            return ID.CompareTo(i2.ID);
        }
    }
}
