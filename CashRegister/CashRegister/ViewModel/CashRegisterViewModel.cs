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
            Receipt.Client = UserManager.GetInstance().User;
        }

        public void AddItemOnReceiptFromEAN(string ean)
        {
            foreach (Item i in Seeder.GetInstance().Items)
            {
                if (i.EAN == ean)
                {
                    AddItemOnReceipt(i);
                    break; // Each EAN is unique, then if we find one don't want to look for another
                }
            }
        }

        public void AddItemOnReceipt(Item item)
        {
            ReceiptLine line = ReceiptLines.FirstOrDefault(i => i.Item == item);

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
            if (CurrentCategory != null && category == CurrentCategory)
            {
                CurrentCategory = null;
                Toolbox.PopulateList(Items, Seeder.GetInstance().Items);

                // Color gesture
                foreach (Category c in Categories)
                {
                    c.ActualColor = c.PrincipalColor;
                }
            }
            else if (CurrentCategory == null || category != CurrentCategory)
            {
                CurrentCategory = category;

                Items.Clear();
                foreach (Item item in CurrentCategory.GetItems())
                {
                    Items.Add(item);
                }

                // Color gesture
                foreach (Category c in Categories)
                {
                    if (c == CurrentCategory)
                    {
                        c.ActualColor = c.PrincipalColor;
                    }
                    else
                    {
                        c.ActualColor = c.SecondaryColor;
                    }
                }
            }
        }

        public void SendMail(string clientMail)
        {
            if (UserManager.GetInstance().IsConnected())
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
                mail.To.Add(clientMail);
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
