﻿using System;
using System.Collections.Generic;
using System.Drawing;
using CashRegister.ViewModel;
using CashRegister.Database;

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
            return RepositoryManager.Instance.ItemRepository.FindAllByCategory(Id);
        }

        public int CompareTo(object obj)
        {
            Category c2 = obj as Category;
            return Id.CompareTo(c2.Id);
        }
    }
}
