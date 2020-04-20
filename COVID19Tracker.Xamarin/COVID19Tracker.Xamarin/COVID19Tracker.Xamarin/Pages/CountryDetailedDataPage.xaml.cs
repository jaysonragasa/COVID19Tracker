using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}