using CashRegister.Model;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using System.Security.Cryptography;

namespace CashRegister.Tools
{
    public class Toolbox
    {
        /// <summary>
        /// Populate an observable collection with the contents of a simple list -> to trigger the INotifyPropertyChanged event
        /// </summary>
        /// <typeparam name="T">Type of the content</typeparam>
        /// <param name="list1">observable collection to populate</param>
        /// <param name="list2">input list</param>
        public static void PopulateList<T>(ObservableCollection<T> list1, IEnumerable<T> list2)
        {
            list1.Clear();
            foreach (T item in list2)
            {
                list1.Add(item);
            }
        }

        /// <summary>
        /// Sort an observable collection 
        /// </summary>
        /// <typeparam name="T">type of the content</typeparam>
        /// <param name="collection">observable collection to sort</param>
        public static void Sort<T>(ObservableCollection<T> collection) where T : IComparable
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count(); i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }
        }

        /// <summary>
        /// Generate the txt file for the receipt
        /// </summary>
        /// <param name="receipt">current receipt</param>
        /// <param name="lines">all receipt lines</param>
        /// <param name="totalPrice">total price of the receipt</param>
        /// <returns></returns>
        public static string GenerateReceiptFile(Receipt receipt, List<ReceiptLine> lines, double totalPrice)
        {
            string fileName = "Receipt.txt";
            string file = Path.Combine(FileSystem.CacheDirectory, fileName);

            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                { 
                }
            }            

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

        /// <summary>
        /// Get the color string from a color
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>color string</returns>
        public static string GetStringFromColor(Color color)
        {
            return $"{color.R}/{color.G}/{color.B}/{color.A}";
        }

        /// <summary>
        /// Get the color from a color string
        /// </summary>
        /// <param name="color">color string</param>
        /// <returns>color</returns>
        public static Color GetColorFromString(string color)
        {
            string[] values = color.Split('/');
            return System.Drawing.Color.FromArgb(int.Parse(values[3]), int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
        }

        /// <summary>
        /// Get RGB color from HSL color
        /// </summary>
        /// <param name="h">Hue</param>
        /// <param name="s">Saturation</param>
        /// <param name="l">Brightness</param>
        /// <returns>RGB color</returns>
        public static Color ColorFromHSL(double h, double s, double l)
        {
            double r = 0, g = 0, b = 0;
            if (l != 0)
            {
                if (s == 0)
                    r = g = b = l;
                else
                {
                    double temp2 = (l < 0.5) ? l * (1.0 + s) : l + s - (l * s);
                    double temp1 = 2.0 * l - temp2;

                    r = GetColorComponent(temp1, temp2, h + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, h);
                    b = GetColorComponent(temp1, temp2, h - 1.0 / 3.0);
                }
            }

            return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }

        /// <summary>
        /// Hash the password using SHA512
        /// </summary>
        /// <param name="password">password</param>
        /// <returns>hash</returns>
        public static string EncryptPassword(string password)
        {
            using (SHA512 sha = SHA512.Create())
            {
                return Convert.ToBase64String(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            }
        }

        /// <summary>
        /// Tool to calculte HSL
        /// </summary>
        /// <param name="temp1"></param>
        /// <param name="temp2"></param>
        /// <param name="temp3"></param>
        /// <returns></returns>
        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;

            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else
                return temp1;
        }
    }
}
