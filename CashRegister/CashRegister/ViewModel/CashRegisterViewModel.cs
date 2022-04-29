using CashRegister.Manager;
using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CashRegister.Tools;
using System.Diagnostics;
using System.Net.Mail;
using CashRegister.Database;

namespace CashRegister.ViewModel
{
    public class CashRegisterViewModel : ViewModelBase
    {
        public List<Item> AllItems { get; }
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
            Categories = RepositoryManager.Instance.CategoryRepository.GetAll();

            AllItems = RepositoryManager.Instance.ItemRepository.GetAll();
            Items = new ObservableCollection<Item>();
            Toolbox.PopulateList(Items, AllItems);

            Receipt = new Receipt
            {
                Client = UserManager.Instance.User
            };
            
            ReceiptLines = new ObservableCollection<ReceiptLine>();
            
            TotalPrice = 0;
        }

        public void AddItemOnReceiptFromEAN(string ean)
        {
            AddItemOnReceipt(RepositoryManager.Instance.ItemRepository.FindByEAN(ean));
        }

        public void AddItemOnReceipt(Item item)
        {
            ReceiptLine line = ReceiptLines.FirstOrDefault(i => i.Item == item);

            if (line != null)
            {
                line.AddItem();
                ReceiptLines.Remove(line);
                ReceiptLines.Add(line);
            }
            else
            {
                line = new ReceiptLine(Receipt, item);
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
        }

        public void RemoveAllSameItemsOnReceipt(ReceiptLine line)
        {
            ReceiptLines.Remove(line);
            Toolbox.Sort(ReceiptLines);
            RefreshTotalPrice();
        }

        public void SelectCategory(Category category)
        {
            if (CurrentCategory != null && category == CurrentCategory)
            {
                CurrentCategory = null;
                Toolbox.PopulateList(Items, AllItems);

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
            // Save into db
            RepositoryManager.Instance.ReceiptRepository.Save(Receipt);
            foreach (ReceiptLine line in ReceiptLines)
            {
                RepositoryManager.Instance.ReceiptLineRepository.Save(line);
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
