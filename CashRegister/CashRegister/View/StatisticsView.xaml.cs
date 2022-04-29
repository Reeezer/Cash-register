using CashRegister.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CashRegister.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsView : ContentPage
    {
        public StatisticsView()
        {
            InitializeComponent();
            BindingContext = new StatisticsViewModel();
        }
    }
}