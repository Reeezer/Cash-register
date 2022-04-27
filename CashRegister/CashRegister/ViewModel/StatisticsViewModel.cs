using CashRegister.Manager;
using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using CashRegister.Tools;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using System.IO;
using System.Diagnostics;
using System.Net.Mail;

namespace CashRegister.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        public double TotalIncome { get; }
        public List<StatisticCategory> Categories { get; }
        public DateTime Date { get; }

        public StatisticsViewModel()
        {
            // TODO remove
            List<Item> items = Seeder.GetInstance().Items.GetRange(0, 3);

            Date = DateTime.Now;
            Categories = new List<StatisticCategory>();

            List<Category> categories = Seeder.GetInstance().Categories;
            foreach(Category category in categories)
            {
                // TODO calculate category total income
                // TODO get the 3 bests items
                // TODO create StatisticCategory and populate Categories

                // TODO increase total income

                // TODO remove
                Categories.Add(new StatisticCategory(category, 10, items));
                TotalIncome += 10;
            }

        }
    }

    public class StatisticCategory
    {
        public Category Category { get; }
        public double TotalIncome { get; }
        public List<Item> BestItems { get; }

        public StatisticCategory(Category category, double totalIncome, List<Item> bestItems)
        {
            Category = category;
            TotalIncome = totalIncome;
            BestItems = bestItems;
        }
    }
}
