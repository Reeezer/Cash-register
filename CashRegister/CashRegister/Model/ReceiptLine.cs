using CashRegister.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.Model
{
    public class ReceiptLine : ViewModelBase, IComparable
    {
        public static int id = 1;

        public int ID { get; }
        public Receipt Receipt { get; }
        public Item Item { get; }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(quantity));
            }
        }

        public ReceiptLine(Receipt receipt, Item item, int quantity=1)
        {
            ID = id++;
            Receipt = receipt;
            Item = item;
            Quantity = quantity;
        }

        public void AddItem(int quantity=1)
        {
            Quantity += quantity;
        }

        public void RemoveItem(int quantity=1)
        {
            Quantity -= quantity;
        }

        public int CompareTo(object obj)
        {
            ReceiptLine c2 = obj as ReceiptLine;
            return Item.Name.CompareTo(c2.Item.Name);
        }
    }
}
