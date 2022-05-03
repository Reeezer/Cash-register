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
    }
}