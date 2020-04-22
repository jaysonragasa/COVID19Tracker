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

            Refresh();
        }

        void Refresh()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var dashboard = ((ViewModelLocator)this.BindingContext).Dashboard;

                await dashboard.RefreshData(dashboard.CurrentFilter);

                dashboard.OnCountryLookupFound += (s, c) =>
                {
                    lvCountries.ScrollTo(c, ScrollToPosition.MakeVisible, true);
                };

                dashboard.OnShowMessage += async (s, c) =>
                {
                    await DisplayAlert(null, c, "ok");
                };
            });
        }
    }
}
