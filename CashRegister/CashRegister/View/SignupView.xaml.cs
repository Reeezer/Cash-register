﻿using CashRegister.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CashRegister.Tools;
using System.Diagnostics;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupView : ContentPage
    {
        public SignupView()
        {
            InitializeComponent();
        }

        public async void ToLogin(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LoginView());
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

                    Debug.WriteLine(encryptedPassword);

                    User user = new User(FirstName.Text, LastName.Text, Email.Text, encryptedPassword, 0);
                    // TODO Save 
                    // TODO if customer -> CashRegisterView, seller -> seller view
                    await Navigation.PushAsync(new CashRegisterView(user));
                }
            }
        }
    }
}