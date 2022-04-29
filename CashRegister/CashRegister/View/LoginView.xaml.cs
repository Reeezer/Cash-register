using CashRegister.Manager;
using CashRegister.Model;
using System;
using System.Linq;
using CashRegister.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CashRegister.Tools;

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
                UserRepository userRepository = RepositoryManager.Instance.UserRepository;
                User user = userRepository.FindByEmail(Email.Text);

                string encryptedPassword = Toolbox.EncryptPassword(Pass.Text);
                if (user != null && user.Password == encryptedPassword)
                {
                    UserManager.Instance.User = user;

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
    }
}