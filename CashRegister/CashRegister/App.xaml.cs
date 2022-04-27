using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using CashRegister.moneyIsEverything;
using System.Diagnostics;
using CashRegister.moneyIsEverything.models;

namespace CashRegister
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Debug.WriteLine("debug");

            ServerData s = PayoutManager.Instance.MakePayement(13, 13, 13, 13, 12.5f, "my-ref");
            Debug.WriteLine("result: " + s);

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
