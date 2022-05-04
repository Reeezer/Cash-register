using System;
using System.Collections.Generic;
using System.Drawing;
using CashRegister.ViewModel;
using CashRegister.Database;
using CashRegister.Tools;

namespace CashRegister.Model
{
    public class Category : ViewModelBase, IComparable
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually
        /// </summary>
        public int Id { get; set; } = 0;

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


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">name of the category</param>
        /// <param name="principalColor">principal color</param>
        /// <param name="secondaryColor">secorndary color</param>
        public Category(string name, Color principalColor, Color secondaryColor)
        {
            Name = name;
            PrincipalColor = principalColor;
            SecondaryColor = secondaryColor;

            ActualColor = principalColor;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">name of the category</param>
        public Category(string name)
        {
            Random random = new Random();
            Name = name;
            PrincipalColor = Toolbox.ColorFromHSL(random.NextDouble(), 0.5, 0.6);
            SecondaryColor = Color.FromArgb(125, PrincipalColor.R, PrincipalColor.G, PrincipalColor.B);
            ActualColor = PrincipalColor;
        }

        public Category()
        {
        }

        public List<Item> GetItems(List<Item> allItems)
        {
            List<Item> items = new List<Item>();
            foreach (Item item in allItems)
            {
                if (item.Category.Id == Id)
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
