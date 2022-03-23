using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CashRegister.Model
{
    public class Category
    {
        public static int id = 1;

        public int ID { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }

        public Category(string name, Color color)
        {
            ID = id++;
            Name = name;
            Color = color;
        }
    }
}
