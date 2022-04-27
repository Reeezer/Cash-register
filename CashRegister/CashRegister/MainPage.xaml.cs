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
using MySqlConnector;
using CashRegister.Manager;

namespace CashRegister
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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
                await DisplayAlert("Logout", "You we're not already connected", "Ok");
            }
            else
            {
                UserManager.GetInstance().User = null;
                await DisplayAlert("Logout", "You have been logged out successfully", "Ok");
            }
        }

        public async void DefaultLogin(object sender, EventArgs args)
        {
            // FIXME Remove
            UserManager.GetInstance().User = new User("Leon", "Muller", "leonmuller@hotmail.fr", "leon", 0);
            await Navigation.PushAsync(new MainMenuView());
        }
    }
}
