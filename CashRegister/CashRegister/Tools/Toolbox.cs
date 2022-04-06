using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
    }
}
