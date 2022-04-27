using CashRegister.View;
using System;
using Xamarin.Forms;
using CashRegister.Services;
using CashRegister.Database;
using CashRegister.Model;
using OpenFoodFacts4Net.ApiClient;
using OpenFoodFacts4Net.Json.Data;
using System.Threading.Tasks;

namespace CashRegister
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Console.WriteLine("Initializing database");
            CashDatabase cashDatabase = CashDatabase.Instance;
            Console.WriteLine("Opening database connection");
            cashDatabase.Open();

            // FIXME debug
            Console.WriteLine("Printing all tables in database");
            cashDatabase.PrintTables();

            UserRepository userRepository = new UserRepository();
            foreach (User user in userRepository.FindAll("Meyer"))
                Console.WriteLine($"{user}");
        }

        public async void ToLogin(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LoginView());
        }

        public async void ToSignup(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SignupView());
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
            User testUser = new User("Wesh", "Man", "bleh@bleh.com", "password", 0);
        }

        private async void btnShowUsers_Clicked(object sender, EventArgs e)
        {
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
