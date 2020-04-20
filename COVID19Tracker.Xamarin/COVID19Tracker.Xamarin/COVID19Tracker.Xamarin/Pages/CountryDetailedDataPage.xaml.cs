﻿using covid19phlib.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COVID19Tracker.Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CountryDetailedDataPage : ContentPage
    {
        public CountryDetailedDataPage()
        {
            InitializeComponent();
        }

        public CountryDetailedDataPage(object param)
        {
            InitializeComponent();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await ((ViewModelLocator)this.BindingContext).Region.RefreshData(param.ToString());
            });
        }
    }
}