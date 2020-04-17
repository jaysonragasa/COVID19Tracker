using covid19phlib.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace COVID19Tracker.Xamarin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await ((ViewModelLocator)this.BindingContext).Dashboard.RefreshData();
            });
        }
    }
}
