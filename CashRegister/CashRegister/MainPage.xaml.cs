using CashRegister.View;
using System;
using Xamarin.Forms;
using CashRegister.Services;
using CashRegister.Database;
using CashRegister.Model;
using OpenFoodFacts4Net.ApiClient;
using OpenFoodFacts4Net.Json.Data;
using System.Threading.Tasks;
using MySqlConnector;
using CashRegister.Manager;

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
            
            Console.WriteLine($"{userRepository.FindById(2)}");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ChangeButtonVisibility();
        }        

        public async void ToLogin(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LoginView());
        }

        public async void ToSignup(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SignupView());
        }

        public async void Logout(object sender, EventArgs args)
        {
            if (UserManager.GetInstance().User == null)
            {
                await DisplayAlert("Logout", "You we're not connected, so it didn't do anything", "Ok");
            }
            else
            {
                UserManager.GetInstance().User = null;
                await DisplayAlert("Logout", "You have been logged out successfully", "Ok");

                ChangeButtonVisibility();
            }
        }

        public void ChangeButtonVisibility()
        {
            if (UserManager.GetInstance().IsConnected())
            {
                LoginButton.IsVisible = false;
                SignupButton.IsVisible = false;
                LogoutButton.IsVisible = true;
                ToMainMenuButton.IsVisible = true;
            }
            else
            {
                LoginButton.IsVisible = true;
                SignupButton.IsVisible = true;
                LogoutButton.IsVisible = false;
                ToMainMenuButton.IsVisible = false;
            }
        }
    }
}
