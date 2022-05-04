using CashRegister.Manager;
using CashRegister.Model;
using System;
using System.Linq;
using CashRegister.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CashRegister.Tools;
using System.Diagnostics;
using System.Net.Mail;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LoginView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check if the credentials are correct and log in the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public async void Login(object sender, EventArgs args)
        {
            if (Email.Text == null || Pass.Text == null)
            {
                await DisplayAlert("Login failed", "Something is missing", "Ok");
            }
            else
            {
                User user = UserRepository.Instance.FindByEmail(Email.Text);

                if (user == null)
                {
                    await DisplayAlert("Login failed", "Email is incorrect", "Ok");
                }
                else
                {
                    string encryptedPassword = Toolbox.EncryptPassword(Pass.Text);
                    if (user.Password == encryptedPassword)
                    {
                        UserManager.Instance.User = user;

                        var navigation = Application.Current.MainPage.Navigation;
                        var lastPage = navigation.NavigationStack.LastOrDefault();

                        await Navigation.PushAsync(new MainMenuView());

                        navigation.RemovePage(lastPage);
                    }
                    else
                    {
                        await DisplayAlert("Login failed", "The password does not fit for this email", "Ok");
                    }
                }
            }
        }
    }
}