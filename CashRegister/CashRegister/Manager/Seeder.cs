using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CashRegister.Manager
{
    public class Seeder
    {
        private static Seeder instance = null;
        private readonly Random random = new Random();
        public List<Item> Items { get; } = new List<Item>();
        public List<Category> Categories { get; } = new List<Category>();

        public Seeder()
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    Category category = new Category($"Category{i}", Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)));
            //    Categories.Add(category);

            //    for (int j = 0; j < 15; j++)
            //    {
            //        Item item = new Item(category, $"Item{Items.Count}", j * i, j + i, "---");
            //        Items.Add(item);
            //    }
            //}
        }

        public static Seeder GetInstance()
        {
            if (instance == null)
            {
                instance = new Seeder();
            }
            return instance;
        }
    }
}
