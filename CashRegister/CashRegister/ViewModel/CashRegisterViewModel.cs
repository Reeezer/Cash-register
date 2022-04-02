using CashRegister.Manager;
using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace CashRegister.ViewModel
{
    public class CashRegisterViewModel : ViewModelBase
    {
        public List<Category> Categories { get; }
        public ObservableCollection<Item> Items { get; }
        public ObservableCollection<ReceiptLine> ReceiptLines { get; }

        private Category currentCategory;
        public Category CurrentCategory
        {
            get => currentCategory;
            set
            {
                currentCategory = value;
                OnPropertyChanged(nameof(currentCategory));
            }
        }

        private User user;
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged(nameof(user));
            }
        }

        private Receipt receipt;
        public Receipt Receipt
        {
            get => receipt;
            set
            {
                receipt = value;
                OnPropertyChanged(nameof(receipt));
            }
        }

        public CashRegisterViewModel()
        {
            Categories = Seeder.GetInstance().Categories;
            Items = new ObservableCollection<Item>();
            PopulateList<Item>(Items, Seeder.GetInstance().Items);
            Receipt = new Receipt();
            ReceiptLines = new ObservableCollection<ReceiptLine>();
        }

        private void PopulateList(ObservableCollection<Item> items1, List<Item> items2)
        {
            throw new NotImplementedException();
        }

        public void AddItemOnReceipt(Item item)
        {
            ReceiptLine line = ReceiptLines.FirstOrDefault(i => i.Item.ID == item.ID);

            if (line != null)
            {
                line.AddItem();
                ReceiptLines.Remove(line);
                ReceiptLines.Add(line);
                // TODO Save
            }
            else
            {
                line = new ReceiptLine(Receipt, item);
                // TODO Save

                ReceiptLines.Add(line);
            }
            ReceiptLines.OrderBy(i => i.Item.Name);
        }

        public void RemoveItemOnReceipt(ReceiptLine line)
        {
            line.RemoveItem();
            ReceiptLines.Remove(line);
            if (line.Quantity > 0)
            {
                ReceiptLines.Add(line);
            }
            ReceiptLines.OrderBy(i => i.Item.Name);
            // TODO Save
        }

        public void RemoveAllSameItemsOnReceipt(ReceiptLine line)
        {
            ReceiptLines.Remove(line);
            ReceiptLines.OrderBy(i => i.Item.Name);
            // TODO Save
        }

        public void SelectCategory(Category category)
        {
            if (CurrentCategory != null && category.ID == CurrentCategory.ID)
            {
                CurrentCategory = null;
                PopulateList<Item>(Items, Seeder.GetInstance().Items);
            }
            else if (CurrentCategory == null || category.ID != CurrentCategory.ID)
            {
                CurrentCategory = category;

                Items.Clear();
                foreach (Item item in CurrentCategory.GetItems())
                {
                    Items.Add(item);
                }
            }
        }
        private static void PopulateList<T>(ObservableCollection<T> list1, IEnumerable<T> list2)
        {
            list1.Clear();
            foreach (T item in list2)
            {
                list1.Add(item);
            }
        }
    }
}
