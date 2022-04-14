using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace CashRegister
{
    public partial class App : Application
    {
        public App()
        {
            // https://dusted.codes/dotenv-in-dotnet
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            DotEnv.Load(dotenv);

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
