using CashRegister.Manager;
using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashRegister.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                UserRepository userRepository = new UserRepository();
                User user = userRepository.FindByEmailPassword(Email.Text, Pass.Text);

                if (user != null)
                {
                    UserManager.GetInstance().User = user;

                    var navigation = Application.Current.MainPage.Navigation;
                    var lastPage = navigation.NavigationStack.LastOrDefault();

                    await Navigation.PushAsync(new MainMenuView());

                    navigation.RemovePage(lastPage);
                }
                else
                {
                    await DisplayAlert("Login failed", "Email or password is incorrect", "Ok");
                }
            }
        }

        public async void ToSignup(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SignupView());
        }
    }
}