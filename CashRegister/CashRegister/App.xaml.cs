using Xamarin.Forms;
using CashRegister.moneyIsEverything;
using System.Diagnostics;
using CashRegister.moneyIsEverything.models;
using System.Threading.Tasks;
using CashRegister.Database;

namespace CashRegister
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Connect all repository to db
            _ = CategoryRepository.Instance;
            _ = ItemRepository.Instance;
            _ = DiscountRepository.Instance;
            _ = ReceiptLineRepository.Instance;
            _ = ReceiptRepository.Instance;
            _ = UserRepository.Instance;

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
