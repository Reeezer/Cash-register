using CashRegister.Manager;
using CashRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                await DisplayAlert("Login failed", "Something is missing", "Okay", "Cancel");
            }
            else
            {
                User user = new User("Leon", "Muller", Email.Text, Pass.Text, 0);
                UserManager.GetInstance().User = user;
                // TODO Verify if customer exists, and get it
                // TODO if curstomer -> CashRegisterView, seller -> seller view

                var navigation = Application.Current.MainPage.Navigation;
                var lastPage = navigation.NavigationStack.LastOrDefault();

                await Navigation.PushAsync(new MainMenuView());

                navigation.RemovePage(lastPage);
            }
        }

        public async void ToSignup(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SignupView());
        }
    }
}