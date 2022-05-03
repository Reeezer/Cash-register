using CashRegister.Database;
using CashRegister.Model;
using CashRegister.Services;
using CashRegister.ViewModel;
using System;
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

        private async void ScanCode(object sender, EventArgs e)
        {
            try
            {
                IQrScanningService scanner = DependencyService.Get<IQrScanningService>();
                string result = await scanner.ScanAsync();

                if (result != null)
                {
                    result = result.Trim(); //removing some eventual 
                    CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
                    cashRegisterVM.AddItemOnReceiptFromEAN(result);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async void ToPayment(object sender, EventArgs args)
        {
            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            
            await Navigation.PushAsync(new PaymentView(cashRegisterVM.TotalPrice, cashRegisterVM.Receipt, cashRegisterVM.ReceiptLines));
        }
    }
}