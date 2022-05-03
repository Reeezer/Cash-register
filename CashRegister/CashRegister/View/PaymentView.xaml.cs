using CashRegister.Database;
using CashRegister.Manager;
using CashRegister.Model;
using CashRegister.moneyIsEverything;
using CashRegister.moneyIsEverything.models;
using CashRegister.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentView : ContentPage
    {
        private readonly double totalPrice;
        private readonly Receipt receipt;
        private readonly List<ReceiptLine> receiptLines;

        public PaymentView()
        {
            InitializeComponent();
        }

        public PaymentView(double totalPrice, Receipt receipt, ObservableCollection<ReceiptLine> receiptLines)
        {
            this.totalPrice = totalPrice;
            this.receipt = receipt;
            this.receiptLines = receiptLines.ToList();
        }

        public async void Pay(object sender, EventArgs args)
        {
            // Check if card is valid
            if (CardNumber.Text == null || CardNumber.Text.Length != 16)
            {
                await DisplayAlert("Error", "Invalid card number", "OK");
                return;
            }

            if (SecurityCode.Text == null || SecurityCode.Text.Length != 3)
            {
                await DisplayAlert("Error", "Invalid security code", "OK");
                return;
            }

            if (ExpirationMonth.Text == null || ExpirationMonth.Text.Length != 2)
            {
                await DisplayAlert("Error", "Invalid expiration month", "OK");
                return;
            }

            if (ExpirationYear.Text == null || ExpirationYear.Text.Length != 4)
            {
                await DisplayAlert("Error", "Invalid expiration year", "OK");
                return;
            }

            if (int.Parse(ExpirationYear.Text) < int.Parse(DateTime.Now.Year.ToString()))
            {
                await DisplayAlert("Error", "Card expired", "OK");
                return;
            }
            else
            {
                if (int.Parse(ExpirationYear.Text) == int.Parse(DateTime.Now.Year.ToString()) && int.Parse(ExpirationMonth.Text) < int.Parse(DateTime.Now.Month.ToString()))
                {
                    await DisplayAlert("Error", "Card expired", "OK");
                    return;
                }
            }

            // Make payment
            try
            {
                Task<ServerData> tsd = PayoutManager.Instance.MakePayement(CardNumber.Text, ExpirationMonth.Text, ExpirationYear.Text, SecurityCode.Text, totalPrice, $"{receipt.Id}");
                ServerData sd = await tsd;
                await DisplayAlert("Success", "Thanks for the payment !", "Ok");
            }
            catch
            {
                await DisplayAlert("Error", "Payment has not been made", "Ok");
                return;
            }

            // Update DB
            RepositoryManager.Instance.ReceiptRepository.Save(receipt);
            foreach (ReceiptLine line in receiptLines)
            {
                RepositoryManager.Instance.ReceiptLineRepository.Save(line);
            }

            User user = UserManager.Instance.User;

            // Send Mail
            try
            {
                string file = Toolbox.GenerateReceiptFile(receipt, receiptLines, totalPrice);

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("cashregister@he-arc.ch"),
                    Subject = $"Receipt from Cas#h Register on {receipt.Date}",
                    Body = "Receipt from Cas# Register"
                };
                if (user.Role == ((int)Role.Customer))
                {
                    mail.To.Add(user.Email);
                }
                else
                {
                    string result = await DisplayPromptAsync("Mail", "Please enter your email to receive the receipt");
                    mail.To.Add(result);
                }                
                mail.Attachments.Add(new Attachment(file));

                SmtpClient SmtpServer = new SmtpClient
                {
                    Port = 25,
                    Host = "smtprel.he-arc.ch",
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("cashregister@he-arc.ch", "azertyuiop")
                };

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error SMTP: {ex}");
            }

            // Go back to main menu
            var navigation = Application.Current.MainPage.Navigation;
            navigation.RemovePage(navigation.NavigationStack.LastOrDefault());
            navigation.RemovePage(navigation.NavigationStack.LastOrDefault());
        }
    }
}