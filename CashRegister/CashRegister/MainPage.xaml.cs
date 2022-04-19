using CashRegister.View;
using System;
using Xamarin.Forms;
using CashRegister.Services;
using Xamarin.Essentials;
using CashRegister.Database;
using CashRegister.Model;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Diagnostics;
using OpenFoodFacts4Net.ApiClient;
using OpenFoodFacts4Net.Csv;
using OpenFoodFacts4Net.Json.Data;
using OpenFoodFacts4Net.Taxonomy.Json;
using OpenFoodFacts4Net.Taxonomy.Json.Data;
using System.Threading.Tasks;

namespace CashRegister
{
    public partial class MainPage : ContentPage
    {
        private SQLiteConnection sqliteco;

        public MainPage()
        {
            InitializeComponent();
            sqliteco = DependencyService.Get<ISQLite>().GetConnection();
            sqliteco.DropTable<User>();
            sqliteco.CreateTable<User>();
        }

        private async void NavigateButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CashRegisterView());
        }

        private async void btnScan_Clicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();

                if (result != null)
                {
                    txtBarcode.Text = result;
                    GetProductAsync(txtBarcode.Text);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void btnTestAddUser_Clicked(object sender, EventArgs e)
        {
            User testUser = new User();
            testUser.FirstName = "Wesh";
            testUser.LastName = "Man";
            testUser.BirthDate = DateTime.Now;
            testUser.Email = "bleh@bleh.com";
            testUser.Role = 0;

            sqliteco.Insert(testUser);
        }

        private async void btnShowUsers_Clicked(object sender, EventArgs e)
        {
            IEnumerable<User> users = (from t in sqliteco.Table<User>() select t).ToList();
            foreach (User user in users)
            {
                Console.WriteLine(user.FirstName);
            }
            await DisplayAlert("Users", "All users displayed in console", "OK");
        }

        private async Task GetProductAsync(string barcode)
        {
            try
            {
                String userAgent = UserAgentHelper.GetUserAgent("OpenFoodFacts4Net.ApiClient.CashRegister", ".Net Standard", "2.0", null);
                Client client = new Client(Constants.BaseUrl, userAgent);
                GetProductResponse productResponse = await client.GetProductAsync(barcode);
                Console.WriteLine(productResponse.Product.ProductName);
                txtArticleDescr.Text = productResponse.Product.ProductName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Article not found");
                txtArticleDescr.Text = "Article not found";
            }

        }

    }
}
