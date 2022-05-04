using CashRegister.ViewModel;
using System;

namespace CashRegister.Model
{
    public class ReceiptLine : ViewModelBase, IComparable
    {
        /// <summary>
        /// IMPORTANT : Do NOT change the ID manually. It is handled by the DB.
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// The receipt linked th the line
        /// </summary>
        public Receipt Receipt { get; set; }
        /// <summary>
        /// The item of the line
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Quantity of items on the line
        /// </summary>
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

        /// <summary>
        /// The price of the items on the line
        /// </summary>
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

        /// <summary>
        /// Contructor of a ReceiptLine
        /// </summary>
        /// <param name="receipt">The Receipt linked to the Line</param>
        /// <param name="item">The item present to  the line</param>
        /// <param name="quantity">The quantity of items on the Line</param>
        public ReceiptLine(Receipt receipt, Item item, int quantity=1)
        {
            Receipt = receipt;
            Item = item;
            Quantity = quantity;
            LinePrice = Quantity * Item.Price;
        }

        /// <summary>
        /// Emnpty constructor
        /// </summary>
        public ReceiptLine()
        {
        }

        /// <summary>
        /// To add an item on the Line
        /// </summary>
        /// <param name="quantity"></param>
        public void AddItem(int quantity=1)
        {
            Quantity += quantity;
            RefreshLinePrice();
        }

        /// <summary>
        /// To remoove an item of the line
        /// </summary>
        /// <param name="quantity"></param>
        public void RemoveItem(int quantity=1)
        {
            Quantity -= quantity;
            RefreshLinePrice();
        }

        /// <summary>
        /// To refresh the price of the line
        /// </summary>
        private void RefreshLinePrice()
        {
            LinePrice = Quantity * Item.Price;
        }

        /// <summary>
        /// Helps to compare two Lines
        /// </summary>
        /// <param name="obj">Line to compare with</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            ReceiptLine c2 = obj as ReceiptLine;
            return Item.CompareTo(c2.Item);
        }
    }
}
