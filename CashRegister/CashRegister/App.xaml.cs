using Xamarin.Forms;
using CashRegister.moneyIsEverything;
using System.Diagnostics;
using CashRegister.moneyIsEverything.models;
using System.Threading.Tasks;

namespace CashRegister
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
