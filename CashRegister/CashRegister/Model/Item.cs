using System;

namespace CashRegister.Model
{
    public class Item : IComparable
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually. It is handled by the DB.
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// Category of the item
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Price of the item
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Quantity available of the item
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Barcode of the item
        /// </summary>
        public string EAN { get; set; }

        /// <summary>
        /// Constructor of an item
        /// </summary>
        /// <param name="category">The category of the item</param>
        /// <param name="name">The name of the item</param>
        /// <param name="price">The price of the item</param>
        /// <param name="quantity">The quantity of the item</param>
        /// <param name="ean">The barcode of the item</param>
        public Item(Category category, string name, double price, int quantity, string ean)
        {
            Category = category;
            Name = name;
            Price = price;
            Quantity = quantity;
            EAN = ean;
        }

        /// <summary>
        /// Empty contructor
        /// </summary>
        public Item()
        {
        }

        /// <summary>
        /// Helps to compare two items
        /// </summary>
        /// <param name="obj">item to compare with</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            Item i2 = obj as Item;
            return Id.CompareTo(i2.Id);
        }
    }
}
