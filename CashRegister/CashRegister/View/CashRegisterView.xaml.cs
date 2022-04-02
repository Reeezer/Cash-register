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
            BindingContext = new CashRegisterViewModel();
        }

        public void SelectCategory(object sender, EventArgs args)
        {
            StackLayout layout = sender as StackLayout;
            Category category = layout.BindingContext as Category;

            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            cashRegisterVM.SelectCategory(category);
        }

        public void SelectItem(object sender, EventArgs args)
        {
            StackLayout layout = sender as StackLayout;
            Item item = layout.BindingContext as Item;

            CashRegisterViewModel cashRegisterVM = BindingContext as CashRegisterViewModel;
            cashRegisterVM.AddItemOnReceipt(item);
        }
    }
}