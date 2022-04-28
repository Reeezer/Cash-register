using CashRegister.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CashRegister.Tools;
using System.Diagnostics;
using CashRegister.Manager;
using System.Linq;
using CashRegister.Database;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupView : ContentPage
    {
        public SignupView()
        {
            InitializeComponent();

            RolePicker.ItemsSource = (System.Collections.IList)Enum.GetValues(typeof(Role)).Cast<Role>();
            RolePicker.SelectedIndex = 0;
        }

        public async void Signup(object sender, EventArgs args)
        {
            if (Email.Text == null || Pass.Text == null || FirstName.Text == null || LastName.Text == null || PassConf.Text == null)
            {
                await DisplayAlert("Sign up failed", "Something is missing", "Okay", "Cancel");
            }
            else
            {
                if (Pass.Text != PassConf.Text)
                {
                    await DisplayAlert("Sign up failed", "Your passwords has to be similar", "Okay", "Cancel");
                }
                else
                {
                    // Password encryption
                    string encryptedPassword = Toolbox.EncryptPassword(Pass.Text);

                    User user = new User(FirstName.Text, LastName.Text, Email.Text, encryptedPassword, 0);
                    UserManager.Instance.User = user;
                    
                    // Save
                    RepositoryManager.Instance.UserRepository.Save(user);

                    var navigation = Application.Current.MainPage.Navigation;
                    var lastPage = navigation.NavigationStack.LastOrDefault();

                    await Navigation.PushAsync(new MainMenuView());

                    navigation.RemovePage(lastPage);
                }
            }
        }
    }
}