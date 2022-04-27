using CashRegister.Model;
using CashRegister.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CashRegisterView : ContentPage
    {
        public CashRegisterView()
        {
            InitializeComponent();

            CashRegisterViewModel cashRegisterVM = new CashRegisterViewModel();
            BindingContext = cashRegisterVM;
        }

        public void SelectCategory(object sender, EventArgs args)
        {
            Button button = sender as Button;
            Category category = button.BindingContext as Category;

            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            cashRegisterVM.SelectCategory(category);
        }

        public void SelectItem(object sender, EventArgs args)
        {
            Button button = sender as Button;
            Item item = button.BindingContext as Item;

            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            cashRegisterVM.AddItemOnReceipt(item);
        }

        public void RemoveItem(object sender, EventArgs args)
        {
            Button button = sender as Button;
            ReceiptLine line = button.BindingContext as ReceiptLine;

            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            cashRegisterVM.RemoveItemOnReceipt(line);
        }

        public void RemoveAllSameItems(object sender, EventArgs args)
        {
            Button button = sender as Button;
            ReceiptLine line = button.BindingContext as ReceiptLine;

            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            cashRegisterVM.RemoveAllSameItemsOnReceipt(line);
        }

        public void SendEmail(object sender, EventArgs args)
        {
            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            _ = SendEmailAsync(cashRegisterVM);
        }

        private async Task SendEmailAsync(CashRegisterViewModel cashRegisterVM)
        {
            string result = await DisplayPromptAsync("Mail", "Please enter your email to receive the receipt");
            cashRegisterVM.SendMail(result);
        }
    }
}