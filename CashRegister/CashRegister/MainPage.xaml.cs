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
        /// <summary>
        /// Constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Change visible buttons on view appearing
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            ChangeButtonVisibility();
        }

        /// <summary>
        /// Navigate to main menu view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public async void ToMainMenu(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new MainMenuView());
        }

        /// <summary>
        /// Navigate to login view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public async void ToLogin(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LoginView());
        }

        /// <summary>
        /// Navigate to sign up view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public async void ToSignup(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SignupView());
        }

        /// <summary>
        /// Logout the current user and change buttons visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
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

        /// <summary>
        /// Change visible buttons if the user is connected or not
        /// </summary>
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
