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
using CashRegister.moneyIsEverything.models;
using System.Threading.Tasks;
using CashRegister.moneyIsEverything;
using OpenFoodFacts4Net.ApiClient;
using Xamarin.Forms;
using OpenFoodFacts4Net.Json.Data;

namespace CashRegister.ViewModel
{
    public class CashRegisterViewModel : ViewModelBase
    {
        public List<Item> AllItems { get; }
        public List<Category> Categories { get; }
        public ObservableCollection<Item> Items { get; }
        public ObservableCollection<ReceiptLine> ReceiptLines { get; }

        private Category currentCategory;

        private GetProductResponse productResponse;

        private Item handledItem;

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
            Categories = CategoryRepository.Instance.GetAll();

            AllItems = ItemRepository.Instance.GetAll();
            Items = new ObservableCollection<Item>();
            Toolbox.PopulateList(Items, AllItems);

            Receipt = new Receipt
            {
                Client = UserManager.Instance.User
            };

            ReceiptLines = new ObservableCollection<ReceiptLine>();

            TotalPrice = 0;
        }

        public async void AddItemOnReceiptFromEAN(string ean)
        {
            Item foundItem = RepositoryManager.Instance.ItemRepository.FindByEAN(ean);

            // If the item doesn't exists in DB
            if (foundItem == null || foundItem.Name.Trim() == "")
                await GetProductAsync(ean);

            else
                //txtArticleDescr.Text = "DB : " + foundItem.Name;

            AddItemOnReceipt(foundItem);

        }

        private async Task GetProductAsync(string barcode)
        {
            try
            {
                String userAgent = UserAgentHelper.GetUserAgent("OpenFoodFacts4Net.ApiClient.CashRegister", ".Net Standard", "2.0", null);
                Client client = new Client(Constants.BaseUrl, userAgent);

                productResponse = await client.GetProductAsync(barcode);
                string foundCat = productResponse.Product.CategoriesTags.First().Substring(3);

                CategoryRepository categoryRepository = RepositoryManager.Instance.CategoryRepository;
                List<Category> cat = categoryRepository.FindAll(foundCat);
                Category newCat = new Category();

                if (cat.Count <= 0)
                {
                    newCat.Name = foundCat;
                    newCat.PrincipalColor = new Color(0.0);
                    newCat.SecondaryColor = new Color(0.0);
                    newCat.ActualColor = new Color(0.0);
                    categoryRepository.Save(newCat);
                }
                else
                {
                    newCat = cat[0];
                }

                handledItem = new Item();
                handledItem.Name = productResponse.Product.ProductName;
                handledItem.Category = newCat;
                handledItem.EAN = barcode;

                //string value = await DisplayPromptAsync("Price", "Please give a price", "OK", "Cancel", null, -1, Keyboard.Numeric, "");
                //Double.TryParse(value, out double price);
                //value = await DisplayPromptAsync("Quantity", "Please give a quantity", "OK", "Cancel", null, -1, Keyboard.Numeric, "");
                //Int32.TryParse(value, out int quantity);

                ItemRepository itemRepository = RepositoryManager.Instance.ItemRepository;
                itemRepository.Save(handledItem);


            }
            catch (Exception)
            {
                handledItem = new Item();
                handledItem.Name = "";
                handledItem.Category = null;
                handledItem.EAN = barcode;
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
                foreach (Item item in CurrentCategory.GetItems(AllItems))
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
