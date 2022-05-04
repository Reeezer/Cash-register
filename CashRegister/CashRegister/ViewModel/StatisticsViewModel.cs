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

        /// <summary>
        /// Constructor: fetch all data and populate lists for the view
        /// </summary>
        public StatisticsViewModel()
        {
            Date = DateTime.Now;
            Categories = new List<StatisticCategory>();
            TotalIncome = 0;

            /* ----- Fetch every thing we need in db ----- */

            Dictionary<int, double> pricesPerCategory = new Dictionary<int, double>();
            Dictionary<int, SortedList<int, double>> pricePerItemPerCategory = new Dictionary<int, SortedList<int, double>>();

            List<Item> allItems = new List<Item>();
            foreach (Receipt receipt in ReceiptRepository.Instance.FindAllByDate(DateTime.Now))
            {
                foreach (ReceiptLine receiptLine in ReceiptLineRepository.Instance.FindAllByReceipt(receipt.Id))
                {
                    allItems.Add(receiptLine.Item);

                    double linePrice = receiptLine.Quantity * receiptLine.Item.Price;
                    Debug.WriteLine(receiptLine.Item.Name + ": " + linePrice + "(" + receiptLine.Quantity + " * " + receiptLine.Item.Price + ")");

                    TotalIncome += linePrice;

                    if (pricesPerCategory.ContainsKey(receiptLine.Item.Category.Id))
                    {
                        pricesPerCategory[receiptLine.Item.Category.Id] += linePrice;
                    }
                    else
                    {
                        pricesPerCategory.Add(receiptLine.Item.Category.Id, linePrice);
                    }

                    if (pricePerItemPerCategory.ContainsKey(receiptLine.Item.Category.Id))
                    {
                        if (pricePerItemPerCategory[receiptLine.Item.Category.Id].ContainsKey(receiptLine.Item.Id))
                        {
                            pricePerItemPerCategory[receiptLine.Item.Category.Id][receiptLine.Item.Id] += linePrice;
                        }
                        else
                        {
                            pricePerItemPerCategory[receiptLine.Item.Category.Id].Add(receiptLine.Item.Id, linePrice);
                        }
                    }
                    else
                    {
                        pricePerItemPerCategory.Add(receiptLine.Item.Category.Id, new SortedList<int, double>());
                        pricePerItemPerCategory[receiptLine.Item.Category.Id].Add(receiptLine.Item.Id, linePrice);
                    }
                }
            }

            /* ----- Populate things for the view ----- */

            List<Category> categories = CategoryRepository.Instance.GetAll();
            foreach (Category category in categories)
            {
                // For the day
                if (pricePerItemPerCategory.ContainsKey(category.Id))
                {
                    List<int> itemsId = pricePerItemPerCategory[category.Id].OrderByDescending(x => x.Value).Select(y => y.Key).Take(3).ToList();
                    List<Item> items = new List<Item>();
                    foreach(int id in itemsId)
                    {
                        items.Add(ItemRepository.Instance.FindById(id));
                    }

                    StatisticCategory statisticCategory = new StatisticCategory(category, pricesPerCategory[category.Id], items);
                    Categories.Add(statisticCategory);
                }
            }
        }
    }

    /// <summary>
    /// Class used for the view
    /// </summary>
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
