using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using CashRegister.moneyIsEverything;

namespace CashRegister
{
    public partial class App : Application
    {
        public App()
        {
            PayoutManager.Instance.MakePayement(13, 13, 13, 13, 12.5f, "myreference");
            InitializeComponent();
            MainPage = new MainPage();
            
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
