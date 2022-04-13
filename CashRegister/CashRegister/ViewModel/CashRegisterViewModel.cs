using CashRegister.Manager;
using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using CashRegister.Tools;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using System.IO;
using System.Diagnostics;
using System.Net.Mail;

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
                OnPropertyChanged(nameof(CurrentCategory));
            }
        }

        private User user;
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private Receipt receipt;
        public Receipt Receipt
        {
            get => receipt;
            set
            {
                receipt = value;
                OnPropertyChanged(nameof(Receipt));
            }
        }

        private double totalPrice;
        public double TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public CashRegisterViewModel()
        {
            Categories = Seeder.GetInstance().Categories;
            Items = new ObservableCollection<Item>();
            Toolbox.PopulateList(Items, Seeder.GetInstance().Items);
            Receipt = new Receipt();
            ReceiptLines = new ObservableCollection<ReceiptLine>();
            TotalPrice = 0;

            // TODO [Debug]
            User = new User("Leon", "Muller", DateTime.Now, "leonmuller@hotmail.fr");
            Receipt.Client = User;
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
            Toolbox.Sort(ReceiptLines);
            RefreshTotalPrice();
        }

        public void RemoveItemOnReceipt(ReceiptLine line)
        {
            line.RemoveItem();
            ReceiptLines.Remove(line);
            if (line.Quantity > 0)
            {
                ReceiptLines.Add(line);
            }
            Toolbox.Sort(ReceiptLines);
            RefreshTotalPrice();
            // TODO Save
        }

        public void RemoveAllSameItemsOnReceipt(ReceiptLine line)
        {
            ReceiptLines.Remove(line);
            Toolbox.Sort(ReceiptLines);
            RefreshTotalPrice();
            // TODO Save
        }

        public void SelectCategory(Category category)
        {
            if (CurrentCategory != null && category.ID == CurrentCategory.ID)
            {
                CurrentCategory = null;
                Toolbox.PopulateList(Items, Seeder.GetInstance().Items);
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

        public void SendMail()
        {
            if (User == null)
            {
                return;
            }

            try
            {
                string file = Toolbox.GenerateReceiptFile(Receipt, ReceiptLines.ToList(), TotalPrice);

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("cashregister@he-arc.ch"),
                    Subject = $"Receipt from Cas#h Register on {Receipt.Date}",
                    Body = "Receipt from Cas# Register"
                };
                mail.To.Add(User.Email);
                mail.Attachments.Add(new Attachment(file));

                SmtpClient SmtpServer = new SmtpClient
                {
                    Port = 25,
                    Host = "smtprel.he-arc.ch",
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("cashregister@he-arc.ch", "azertyuiop")
                };

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error SMTP: {ex}");
            }
        }

        private void RefreshTotalPrice()
        {
            TotalPrice = 0;
            foreach (ReceiptLine line in ReceiptLines)
            {
                TotalPrice += line.LinePrice;
            }
        }
    }
}
