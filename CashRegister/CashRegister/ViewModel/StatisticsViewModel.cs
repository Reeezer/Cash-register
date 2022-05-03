using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using CashRegister.Database;
using System.Diagnostics;

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
            foreach(Receipt receipt in ReceiptRepository.Instance.FindAllByDate(DateTime.Now))
            {
                foreach (ReceiptLine receiptLine in ReceiptLineRepository.Instance.FindAllByReceipt(receipt.Id))
                {
                    allItems.Add(receiptLine.Item);

                    TotalIncome += receiptLine.LinePrice;

                    if (pricesPerCategory.ContainsKey(receiptLine.Item.Category))
                    {
                        pricesPerCategory[receiptLine.Item.Category] += receiptLine.LinePrice;
                    }
                    else
                    {
                        pricesPerCategory.Add(receiptLine.Item.Category, receiptLine.LinePrice);
                    }

                    if (pricePerItemPerCategory.ContainsKey(receiptLine.Item.Category))
                    {
                        if (pricePerItemPerCategory[receiptLine.Item.Category].ContainsKey(receiptLine.Item))
                        {
                            pricePerItemPerCategory[receiptLine.Item.Category][receiptLine.Item] += receiptLine.LinePrice;
                        }
                        else
                        {
                            pricePerItemPerCategory[receiptLine.Item.Category].Add(receiptLine.Item, receiptLine.LinePrice);
                        }
                    }
                    else
                    {
                        pricePerItemPerCategory.Add(receiptLine.Item.Category, new SortedList<Item, double>());
                        pricePerItemPerCategory[receiptLine.Item.Category].Add(receiptLine.Item, receiptLine.LinePrice);
                    }              
                }
            }

            /* ----- Populate things for the view ----- */

            List<Category> categories = CategoryRepository.Instance.GetAll();
            foreach (Category category in categories)
            {
                Debug.WriteLine(category.Name);
                // For the day
                if (pricePerItemPerCategory.Keys.Contains(category))
                {
                    List<Item> items = pricePerItemPerCategory[category].OrderByDescending(x => x.Value).Select(y => y.Key).Take(3).ToList();
                    Categories.Add(new StatisticCategory(category, pricesPerCategory[category], items));
                }
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
