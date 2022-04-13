using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using CashRegister.Manager;

namespace CashRegister.Model
{
    public class Category
    {
        private static int id = 1;

        public int ID { get; }
        public string Name { get; }
        public Color Color { get; }

        public Category(string name, Color color)
        {
            ID = id++;
            Name = name;
            Color = color;
        }

        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();

            foreach (Item item in Seeder.GetInstance().Items)
            {
                if (item.Category.ID == ID)
                {
                    items.Add(item);
                }
            }

            return items;
        }
    }
}
