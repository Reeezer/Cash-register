﻿using Xamarin.Forms;
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

            TryServer();
            
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

        private async void TryServer()
        {
            try
            {
                Task<ServerData> tsd = PayoutManager.Instance.MakePayement(13, 13, 13, 13, 12.5f, "my-ref");
                ServerData sd = await tsd;
                Debug.WriteLine("result:\n" + sd);
            }
            catch
            {
                Debug.WriteLine("Error Try server");
            }
        }
    }
}
