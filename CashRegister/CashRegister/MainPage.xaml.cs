using CashRegister.View;
using System;
using Xamarin.Forms;
using CashRegister.Database;
using CashRegister.Manager;
using CashRegister.moneyIsEverything;
using CashRegister.moneyIsEverything.models;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CashRegister
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            CashDatabase cashDatabase = CashDatabase.Instance;
            cashDatabase.Open();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ChangeButtonVisibility();
        }

        public async void ToMainMenu(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new MainMenuView());
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
            if (UserManager.Instance.User == null)
            {
                await DisplayAlert("Logout", "You we're not connected, so it didn't do anything", "Ok");
            }
            else
            {
                UserManager.Instance.User = null;
                await DisplayAlert("Logout", "You have been logged out successfully", "Ok");

                ChangeButtonVisibility();
            }
        }

        public void ChangeButtonVisibility()
        {
            if (UserManager.Instance.IsConnected())
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
