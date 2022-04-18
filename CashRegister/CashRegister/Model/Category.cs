using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using CashRegister.Manager;
using CashRegister.ViewModel;

namespace CashRegister.Model
{
    public class Category : ViewModelBase, IComparable
    {
        private static int id = 1;

        public int ID { get; }
        public string Name { get; }
        public Color PrincipalColor { get; }
        public Color SecondaryColor { get; }

        private Color actualColor;
        public Color ActualColor
        {
            get => actualColor;
            set
            {
                actualColor = value;
                OnPropertyChanged(nameof(ActualColor));
            }
        }

        public Category(string name, Color principalColor, Color secondaryColor)
        {
            ID = id++;
            Name = name;
            PrincipalColor = principalColor;
            SecondaryColor = secondaryColor;

            ActualColor = principalColor;
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

        public int CompareTo(object obj)
        {
            Category c2 = obj as Category;
            return ID.CompareTo(c2.ID);
        }
    }
}
