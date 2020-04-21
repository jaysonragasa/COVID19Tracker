
using covid19phlib.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COVID19Tracker.Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegionPage : ContentPage
    {
        public RegionPage()
        {
            InitializeComponent();
        }

        public RegionPage(object param)
        {
            InitializeComponent();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert(null, "This may take several seconds to load due to the volume of data.", "ok");

                await ((ViewModelLocator)this.BindingContext).Region.RefreshData(param.ToString());  
            });

            ((ViewModelLocator)this.BindingContext).Region.OnRegionLookupFound += (s, c) =>
            {
                lvCountries.ScrollTo(c, ScrollToPosition.MakeVisible, true);
            };

            ((ViewModelLocator)this.BindingContext).Region.OnShowMessage += async (s, c) =>
            {
                await DisplayAlert(null, c, "ok");
            };
        }
    }
}