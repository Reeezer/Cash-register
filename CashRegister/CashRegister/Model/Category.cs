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
        /// IMPORTANT : Do NOT change the ID manually. It is handled by the DB.
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// Name of the category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Main color of the category
        /// </summary>
        public Color PrincipalColor { get; set; }

        /// <summary>
        /// Secondary color in case two categories has the same main color.
        /// </summary>
        public Color SecondaryColor { get; set; }

        /// <summary>
        /// The color displayed in the screen
        /// </summary>
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
        /// Constructor with name and two main colors
        /// </summary>
        /// <param name="name">The name of the category</param>
        /// <param name="principalColor">The principal color</param>
        /// <param name="secondaryColor">The secondary color</param>
        public Category(string name, Color principalColor, Color secondaryColor)
        {
            Name = name;
            PrincipalColor = principalColor;
            SecondaryColor = secondaryColor;

            ActualColor = principalColor;
        }

        /// <summary>
        /// Constructor with only the name. The colors are picked randomly
        /// </summary>
        /// <param name="name">The name of the category</param>
        public Category(string name)
        {
            Random random = new Random();
            Name = name;
            PrincipalColor = Toolbox.ColorFromHSL(random.NextDouble(), 0.5, 0.6);
            SecondaryColor = Color.FromArgb(125, PrincipalColor.R, PrincipalColor.G, PrincipalColor.B);
            ActualColor = PrincipalColor;
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Category()
        {
        }

        /// <summary>
        /// Gets all the items linked to the category
        /// </summary>
        /// <param name="allItems">List of all items selected</param>
        /// <returns></returns>
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

        /// <summary>
        /// Helps to compare two categories
        /// </summary>
        /// <param name="obj">category to compare with</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            Category c2 = obj as Category;
            return Id.CompareTo(c2.Id);
        }
    }
}
