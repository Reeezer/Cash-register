using CashRegister.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CashRegister.Model
{
    public class ReceiptLine : ViewModelBase, IComparable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public Receipt Receipt { get; }
        public Item Item { get; }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private double linePrice;
        public double LinePrice
        {
            get => linePrice;
            set
            {
                linePrice = value;
                OnPropertyChanged(nameof(LinePrice));
            }
        }

        public ReceiptLine(Receipt receipt, Item item, int quantity=1)
        {
            Receipt = receipt;
            Item = item;
            Quantity = quantity;
            LinePrice = Quantity * Item.Price;
        }

        public ReceiptLine()
        {
        }

        public void AddItem(int quantity=1)
        {
            Quantity += quantity;
            RefreshLinePrice();
        }

        public void RemoveItem(int quantity=1)
        {
            Quantity -= quantity;
            RefreshLinePrice();
        }

        private void RefreshLinePrice()
        {
            LinePrice = Quantity * Item.Price;
        }

        public int CompareTo(object obj)
        {
            ReceiptLine c2 = obj as ReceiptLine;
            return Item.CompareTo(c2.Item);
        }
    }
}
