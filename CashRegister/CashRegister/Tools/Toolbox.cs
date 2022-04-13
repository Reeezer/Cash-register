using CashRegister.Model;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace CashRegister.Tools
{
    public class Toolbox
    {
        public static void PopulateList<T>(ObservableCollection<T> list1, IEnumerable<T> list2)
        {
            list1.Clear();
            foreach (T item in list2)
            {
                list1.Add(item);
            }
        }

        public static void Sort<T>(ObservableCollection<T> collection) where T : IComparable
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count(); i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }
        }

        public static string GenerateReceiptFile(Receipt receipt, List<ReceiptLine> lines, double totalPrice)
        {
            string fileName = "Receipt.txt";
            string file = Path.Combine(FileSystem.CacheDirectory, fileName); // TODO Delete it before creating it
            string text = "";

            // Receipt informations
            text += $"--- Receipt from Cas# Register ---\n";
            text += $"On date: {receipt.Date}\n";
            text += $"On the email address: {receipt.Client.Email}\n\n";

            // Each item
            text += $"Item\t\t.-/unit\tQuantity\tTotal price\n";
            foreach (ReceiptLine line in lines)
            {
                text += $"{line.Item.Name}\t\t{line.Item.Price}.-\t{line.Item.Quantity}\t\t{line.LinePrice}.-\n";
            }
            text += $"\n";

            // Discounts
            if (receipt.Discount != null)
            {
                text += $"Discount on '{receipt.Discount.Category}' by {receipt.Discount.Percentage}%\n\n";
            }

            // Total price
            text += $"Total price: {totalPrice}.-\n";

            File.WriteAllText(file, text);

            return file;
        }
    }
}
