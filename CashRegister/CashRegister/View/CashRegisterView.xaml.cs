using CashRegister.Database;
using CashRegister.Manager;
using CashRegister.Model;
using CashRegister.Services;
using CashRegister.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CashRegisterView : ContentPage
    {
        public CashRegisterView()
        {
            InitializeComponent();

            CashRegisterViewModel cashRegisterVM = new CashRegisterViewModel();
            BindingContext = cashRegisterVM;
        }

        public void SelectCategory(object sender, EventArgs args)
        {
            Button button = sender as Button;
            Category category = button.BindingContext as Category;

            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            cashRegisterVM.SelectCategory(category);
        }

        public void SelectItem(object sender, EventArgs args)
        {
            Button button = sender as Button;
            Item item = button.BindingContext as Item;

            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            cashRegisterVM.AddItemOnReceipt(item);
        }

        public void RemoveItem(object sender, EventArgs args)
        {
            Button button = sender as Button;
            ReceiptLine line = button.BindingContext as ReceiptLine;

            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            cashRegisterVM.RemoveItemOnReceipt(line);
        }

        public void RemoveAllSameItems(object sender, EventArgs args)
        {
            Button button = sender as Button;
            ReceiptLine line = button.BindingContext as ReceiptLine;

            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            cashRegisterVM.RemoveAllSameItemsOnReceipt(line);
        }

        private async void ScanCode(object sender, EventArgs e)
        {
            try
            {
                IQrScanningService scanner = DependencyService.Get<IQrScanningService>();
                string result = await scanner.ScanAsync();

                if (result != null && result.Length > 0)
                {
                    result = result.Trim(); //removing some eventual 
                    CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
                    Item resultItem = await cashRegisterVM.AddItemOnReceiptFromEAN(result);

                    if (resultItem != null)
                    {
                        if (UserManager.Instance.User.Role != ((int)Role.Customer))
                        {
                            int start = 1;
                            int end;
                            //Verifying how to complete the Item:
                            if (resultItem.Category == null)
                            {
                                end = 4;
                                //setting the Name 
                                string name = await DisplayPromptAsync($"Name {start}/{end}", "Please give a name", "OK", null, null, -1, Keyboard.Default, "");
                                start++;
                                resultItem.Name = name;

                                //setting the Category 
                                string category = await DisplayPromptAsync($"Category {start}/{end}", "Please give a category", "OK", null, null, -1, Keyboard.Default, "");
                                start++;
                                Category newCat = new Category(category);
                                CategoryRepository catRepository = CategoryRepository.Instance;
                                List<Category> foundCat = catRepository.FindAll(newCat.Name);
                                resultItem.Category = newCat;

                                if (foundCat.Count == 0)
                                {
                                    catRepository.Save(newCat);
                                    resultItem.Category = newCat;
                                }
                                else
                                {
                                    resultItem.Category = foundCat[0];
                                }
                            }
                            else
                            {
                                end = 2;
                            }

                            //setting the Price
                            string value = await DisplayPromptAsync($"Price {start}/{end}", "Please give a price", "OK", null, null, -1, Keyboard.Numeric, "");
                            double.TryParse(value, out double price);
                            start++;
                            resultItem.Price = price;

                            //setting the Quantity
                            value = await DisplayPromptAsync($"Quantity {start}/{end}", "Please give a quantity", "OK", null, null, -1, Keyboard.Numeric, "");
                            int.TryParse(value, out int quantity);
                            resultItem.Quantity = quantity;

                            ItemRepository itemRepository = ItemRepository.Instance;
                            itemRepository.Save(resultItem);
                            
                            cashRegisterVM.FetchAllData();
                            cashRegisterVM.AddItemOnReceiptFromEAN(result);
                        }
                        else
                        {
                            await DisplayAlert("Invalid Item", $"The item {result} does not exist.", "Ok");
                        }
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Invalid Item", $"The item does not exist.", "Ok");
                throw;
            }
        }

        public async void ToPayment(object sender, EventArgs args)
        {
            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            if (cashRegisterVM.TotalPrice == 0)
            {
                await DisplayAlert("Error", "You first need to add items to the receipt", "Ok");
                return;
            }
            
            await Navigation.PushAsync(new PaymentView(cashRegisterVM.TotalPrice, cashRegisterVM.Receipt, cashRegisterVM.ReceiptLines));
        }
    }
}