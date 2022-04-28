﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CashRegister.View;
using CashRegister.Services;
using Xamarin.Essentials;
using CashRegister.Database;
using CashRegister.Model;
using SQLite;
using System.Diagnostics;
using OpenFoodFacts4Net.ApiClient;
using OpenFoodFacts4Net.Csv;
using OpenFoodFacts4Net.Json.Data;
using OpenFoodFacts4Net.Taxonomy.Json;
using OpenFoodFacts4Net.Taxonomy.Json.Data;
using MySqlConnector;
using CashRegister.Manager;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuView : ContentPage
    {
        private readonly SQLiteConnection sqliteco;
        private readonly MySqlConnection mysqlco;

        public MainMenuView()
        {
            InitializeComponent();

            //sqliteco = DependencyService.Get<ISQLite>().GetConnection();

            //mysqlco = DependencyService.Get<IMySQL>().GetConnection(myVar);
            //sqliteco.DropTable<User>(); // FIXME
            //sqliteco.CreateTable<User>();
            //mysqlco.OpenAsync();

            if (UserManager.GetInstance().User.Role == ((int)Role.Customer))
            {
                StatisticsButton.IsVisible = false;
            }
        }

        public async void ToCashRegister(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CashRegisterView());
        }

        public async void ToStatistics(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new StatisticsView());
        }

        private async void Scan(object sender, EventArgs e)
        {
            try
            {
                IQrScanningService scanner = DependencyService.Get<IQrScanningService>();
                string result = await scanner.ScanAsync();

                if (result != null)
                {
                    txtBarcode.Text = result;
                    GetProductAsync(txtBarcode.Text);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void AddUser(object sender, EventArgs e)
        {
            User testUser = new User("Wesh", "Man", "bleh@bleh.com", "password", 0);

            sqliteco.Insert(testUser);
        }

        private async void ShowUsers(object sender, EventArgs e)
        {
            IEnumerable<User> users = (from t in sqliteco.Table<User>() select t).ToList();
            foreach (User user in users)
            {
                Console.WriteLine(user.Id + " " + user.FirstName);
            }
            await DisplayAlert("Users", "All users displayed in console", "OK");
        }

        private async Task GetProductAsync(string barcode)
        {
            try
            {
                String userAgent = UserAgentHelper.GetUserAgent("OpenFoodFacts4Net.ApiClient.CashRegister", ".Net Standard", "2.0", null);
                Client client = new Client(Constants.BaseUrl, userAgent);
                GetProductResponse productResponse = await client.GetProductAsync(barcode);
                Console.WriteLine(productResponse.Product.ProductName);
                txtArticleDescr.Text = productResponse.Product.ProductName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Article not found");
                txtArticleDescr.Text = "Article not found";
            }
        }
    }
}