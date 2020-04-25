using covid19phlib.Enums;
using covid19phlib.ViewModels;
using System;
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
                dashboard.ReloadOnFilterSelection = false;
                dashboard.AddListFilter();

                var app = (App)Application.Current;
                var selectedFilter = app.Settings.GetSetting("SelectedFilter", "GLOBAL");

                var filter = (Enums_ListFilter)Enum.Parse(typeof(Enums_ListFilter), selectedFilter.ToString());

                if (filter == Enums_ListFilter.GLOBAL)
                {
                    dashboard.SelectedFilter = dashboard.ListFilter[0];
                }
                else if(filter == Enums_ListFilter.ASEAN)
                {
                    dashboard.SelectedFilter = dashboard.ListFilter[1];
                }

                await dashboard.RefreshData(filter);

                dashboard.OnCountryLookupFound += (s, c) =>
                {
                    lvCountries.ScrollTo(c, ScrollToPosition.MakeVisible, true);
                };

                dashboard.OnShowMessage += async (s, c) =>
                {   
                    await DisplayAlert(null, c, "ok");
                };

                dashboard.ReloadOnFilterSelection = true;
            });
        }
    }
}
