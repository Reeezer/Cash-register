using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using CashRegister.Tools;
using SQLite;
using CashRegister.Database;
using Xamarin.Forms;

namespace CashRegister.Manager
{
    public class Seeder
    {
        private static Seeder instance = null;

        public List<Item> Items { get; } = new List<Item>();
        public List<Category> Categories { get; } = new List<Category>();

        public Seeder()
        {
            int nbCategories = 10;

            for (int i = 0; i < nbCategories; i++)
            {
                System.Drawing.Color principalColor = Toolbox.ColorFromHSL(1.0 / nbCategories * i, 0.5, 0.6);
                System.Drawing.Color secondaryColor = System.Drawing.Color.FromArgb(125, principalColor.R, principalColor.G, principalColor.B);

                Category category = new Category($"Category{i}", principalColor, secondaryColor);
                Categories.Add(category);

                for (int j = 0; j < 15; j++)
                {
                    Item item = new Item(category, $"Item{Items.Count}", j * i, j + i, "---");
                    Items.Add(item);
                }
            }
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
