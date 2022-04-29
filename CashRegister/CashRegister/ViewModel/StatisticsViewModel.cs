using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using CashRegister.Database;

namespace CashRegister.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        public double TotalIncome { get; }
        public List<StatisticCategory> Categories { get; }
        public DateTime Date { get; }

        public StatisticsViewModel()
        {
            Date = DateTime.Now;
            Categories = new List<StatisticCategory>();
            TotalIncome = 0;

            /* ----- Fetch every thing we need in db ----- */

            Dictionary<Category, double> pricesPerCategory = new Dictionary<Category, double>();
            Dictionary<Category, SortedList<Item, double>> pricePerItemPerCategory = new Dictionary<Category, SortedList<Item, double>>();

            List<Item> allItems = new List<Item>();
            foreach(Receipt receipt in RepositoryManager.Instance.ReceiptRepository.FindAllByDate(DateTime.Now))
            {
                foreach (ReceiptLine receiptLine in RepositoryManager.Instance.ReceiptLineRepository.FindAllByReceipt(receipt.Id))
                {
                    allItems.Add(receiptLine.Item);
                    
                    TotalIncome += receiptLine.LinePrice;
                    
                    pricesPerCategory[receiptLine.Item.Category] += receiptLine.LinePrice;
                    pricePerItemPerCategory[receiptLine.Item.Category][receiptLine.Item] += receiptLine.LinePrice;
                }
            }

            /* ----- Populate things for the view ----- */

            List<Category> categories = RepositoryManager.Instance.CategoryRepository.GetAll();
            foreach (Category category in categories)
            {
                // For the day
                List<Item> items = pricePerItemPerCategory[category].OrderByDescending(x => x.Value).Select(y => y.Key).Take(3).ToList();
                Categories.Add(new StatisticCategory(category, pricesPerCategory[category], items));
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
