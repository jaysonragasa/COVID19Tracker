using covid19phlib.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COVID19Tracker.Xamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityPage : ContentPage
    {
        public CityPage()
        {
            InitializeComponent();
        }

        public CityPage(object param)
        {
            InitializeComponent();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await ((ViewModelLocator)this.BindingContext).City.RefreshData(param.ToString());

                ((ViewModelLocator)this.BindingContext).City.OnCityLookupFound += (s, c) =>
                {
                    lvCountries.ScrollTo(c, ScrollToPosition.MakeVisible, true);
                };

                ((ViewModelLocator)this.BindingContext).City.OnShowMessage += async (s, c) =>
                {
                    await DisplayAlert(null, c, "ok");
                };
            });
        }
    }
}