using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using CashRegister.Manager;
using CashRegister.ViewModel;
using SQLite;

namespace CashRegister.Model
{
    public class Category : ViewModelBase, IComparable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public Color PrincipalColor { get; set; }
        public Color SecondaryColor { get; set; }

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
            Name = name;
            PrincipalColor = principalColor;
            SecondaryColor = secondaryColor;

            ActualColor = principalColor;
        }

        public Category()
        {
        }

        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();

            foreach (Item item in Seeder.GetInstance().Items)
            {
                if (item.Category == this)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        public int CompareTo(object obj)
        {
            Category c2 = obj as Category;
            return Id.CompareTo(c2.Id);
        }
    }
}
