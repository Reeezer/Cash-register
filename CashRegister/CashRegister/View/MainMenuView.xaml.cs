using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CashRegister.Services;
using CashRegister.Database;
using CashRegister.Model;
using OpenFoodFacts4Net.ApiClient;
using OpenFoodFacts4Net.Json.Data;
using CashRegister.Manager;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuView : ContentPage
    {
        GetProductResponse productResponse;

        public MainMenuView()
        {
            InitializeComponent();

            if (UserManager.Instance.User.Role == ((int)Role.Customer))
            {
                StatisticsButton.IsVisible = false;
            }
        }

        public async void ToCashRegister(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CashRegisterView());
        }

        public async void ToStatistics(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new StatisticsView());
        }

        private async void Scan(object sender, EventArgs e)
        {
            try
            {
                IQrScanningService scanner = DependencyService.Get<IQrScanningService>();
                string result = await scanner.ScanAsync();

                if (result != null)
                {
                    txtBarcode.Text = result.Trim();

                    ItemRepository itemRepository = RepositoryManager.Instance.ItemRepository;
                    //itemRepository.cashDatabase.Open();
                    Item foundItem = itemRepository.FindByEAN(txtBarcode.Text);
                    //itemRepository.cashDatabase.Close();
                    
                    // If the item doesn't exists in DB
                    if (foundItem == null || foundItem.Name.Trim() == "")
                        await GetProductAsync(txtBarcode.Text);

                    else
                        txtArticleDescr.Text = "DB : " + foundItem.Name;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task GetProductAsync(string barcode)
        {
            try
            {
                String userAgent = UserAgentHelper.GetUserAgent("OpenFoodFacts4Net.ApiClient.CashRegister", ".Net Standard", "2.0", null);
                Client client = new Client(Constants.BaseUrl, userAgent);
                productResponse = await client.GetProductAsync(barcode);
                string foundCat = productResponse.Product.CategoriesTags.First();

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

                string value = await DisplayPromptAsync("Price", "Please give a price","OK","Cancel",null,-1,Keyboard.Numeric,"");
                double price = 0.0;
                Double.TryParse(value, out price);
                value = await DisplayPromptAsync("Quantity", "Please give a quantity", "OK", "Cancel", null, -1, Keyboard.Numeric,"");
                int quantity = 0;
                Int32.TryParse(value, out quantity);

                Item newItem = new Item
                {
                    Name = productResponse.Product.ProductName,
                    Category = newCat,
                    EAN = barcode.Trim(),
                    Price = price,
                    Quantity = quantity
                };

                ItemRepository itemRepository = RepositoryManager.Instance.ItemRepository;
                itemRepository.Save(newItem);

                txtArticleDescr.Text = productResponse.Product.ProductName;
            }
            catch (Exception)
            {
                txtArticleDescr.Text = "Article not found";
            }

        }
    }
}