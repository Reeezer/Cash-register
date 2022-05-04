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

        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentView(double totalPrice, Receipt receipt, ObservableCollection<ReceiptLine> receiptLines)
        {
            InitializeComponent();

            this.totalPrice = totalPrice;
            this.receipt = receipt;
            this.receiptLines = receiptLines.ToList();

            Price.Text = $"Price: {totalPrice}.-";
        }

        /// <summary>
        /// Check if the credentials are correct, make the payment and send the receipt via mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public async void Pay(object sender, EventArgs args)
        {
            // Check if card is valid
            if (CardNumber.Text == null || CardNumber.Text.Length != 16)
            {
                await DisplayAlert("Error", "Invalid card number, has to be 16 digits", "Ok");
                return;
            }

            if (SecurityCode.Text == null || SecurityCode.Text.Length != 3)
            {
                await DisplayAlert("Error", "Invalid security code, has to be 3 digits", "Ok");
                return;
            }

            if (ExpirationMonth.Text == null || ExpirationMonth.Text.Length != 2 || int.Parse(ExpirationMonth.Text) > 12 || int.Parse(ExpirationMonth.Text) < 1)
            {
                await DisplayAlert("Error", "Invalid expiration month, has to be 2 digits", "Ok");
                return;
            }

            if (ExpirationYear.Text == null || ExpirationYear.Text.Length != 4)
            {
                await DisplayAlert("Error", "Invalid expiration year, has to be 4 digits", "Ok");
                return;
            }

            if (int.Parse(ExpirationYear.Text) < int.Parse(DateTime.Now.Year.ToString()))
            {
                await DisplayAlert("Error", "Card expired", "Ok");
                return;
            }
            else
            {
                if (int.Parse(ExpirationYear.Text) == int.Parse(DateTime.Now.Year.ToString()) && int.Parse(ExpirationMonth.Text) < int.Parse(DateTime.Now.Month.ToString()))
                {
                    await DisplayAlert("Error", "Card expired", "Ok");
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
                await DisplayAlert("Error", "There has been a problem with the payment", "Ok");
                return;
            }

            // Update DB
            if (receipt.Discount == null)
            {
                Discount discount = new Discount(DateTime.Now, DateTime.Now, receiptLines.FirstOrDefault().Item.Category, 0);
                receipt.Discount = discount;
                DiscountRepository.Instance.Save(discount);
            }
            ReceiptRepository.Instance.Save(receipt);
            foreach (ReceiptLine line in receiptLines)
            {
                Debug.WriteLine(line.Item.Name + ": " + line.Quantity + " * " + line.Item.Price);
                ReceiptLineRepository.Instance.Save(line);
            }

            // Send Mail
            User user = UserManager.Instance.User;
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
                    if (result != null)
                    {
                        mail.To.Add(result.Trim());
                    }
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