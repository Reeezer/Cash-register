using CashRegister.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CashRegister.Tools;
using CashRegister.Manager;
using System.Linq;
using CashRegister.Database;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupView : ContentPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SignupView()
        {
            InitializeComponent();

            RolePicker.ItemsSource = (System.Collections.IList)Enum.GetValues(typeof(Role)).Cast<Role>();
            RolePicker.SelectedIndex = 0;
        }

        /// <summary>
        /// Check if the credentials are valid and create a new user, and log in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public async void Signup(object sender, EventArgs args)
        {
            if (Email.Text == null || Pass.Text == null || FirstName.Text == null || LastName.Text == null || PassConf.Text == null)
            {
                await DisplayAlert("Sign up failed", "Something is missing", "Ok");
            }
            else
            {
                if (Pass.Text != PassConf.Text)
                {
                    await DisplayAlert("Sign up failed", "Your passwords has to be similar", "Ok");
                }
                else
                {
                    User u = UserRepository.Instance.FindByEmail(Email.Text);

                    if (u == null)
                    {
                        // Password encryption
                        string encryptedPassword = Toolbox.EncryptPassword(Pass.Text);

                        User user = new User(FirstName.Text, LastName.Text, Email.Text, encryptedPassword, (Role)RolePicker.SelectedItem);
                        UserManager.Instance.User = user;

                        // Save
                        UserRepository.Instance.Save(user);

                        var navigation = Application.Current.MainPage.Navigation;
                        var lastPage = navigation.NavigationStack.LastOrDefault();

                        await Navigation.PushAsync(new MainMenuView());

                        navigation.RemovePage(lastPage);
                    }
                    else
                    {
                        await DisplayAlert("Sign up failed", "This email already exists", "Ok");
                    }
                }
            }
        }
    }
}