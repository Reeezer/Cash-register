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
    public partial class SignupView : ContentPage
    {
        public SignupView()
        {
            InitializeComponent();
        }

        public async void ToLogin(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LoginView());
        }

        public async void Signup(object sender, EventArgs args)
        {
            if (Email.Text == null || Pass.Text == null || FirstName.Text == null || LastName.Text == null)
            {
                await DisplayAlert("Login failed", "Something is missing", "Okay", "Cancel");
            }
            else
            {
                User user = new User(FirstName.Text, LastName.Text, Email.Text, Pass.Text, 0);
                // TODO Save 
                // TODO encrypt password
                // TODO if curstomer -> CashRegisterView, seller -> seller view
                await Navigation.PushAsync(new CashRegisterView(user));
            }
        }
    }
}