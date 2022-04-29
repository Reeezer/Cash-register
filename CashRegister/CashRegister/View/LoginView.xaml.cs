using CashRegister.Manager;
using CashRegister.Model;
using System;
using System.Linq;
using CashRegister.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CashRegister.Tools;
using System.Diagnostics;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public async void Login(object sender, EventArgs args)
        {
            if (Email.Text == null || Pass.Text == null)
            {
                await DisplayAlert("Login failed", "Something is missing", "Ok");
            }
            else
            {
                User user = RepositoryManager.Instance.UserRepository.FindByEmail(Email.Text);

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