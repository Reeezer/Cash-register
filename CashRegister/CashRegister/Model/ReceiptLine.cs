using CashRegister.ViewModel;
using System;

namespace CashRegister.Model
{
    public class ReceiptLine : ViewModelBase, IComparable
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually
        /// </summary>
        public int Id { get; set; } = 0;

        public Receipt Receipt { get; set; }
        public Item Item { get; set; }

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
