using covid19phlib.Interfaces;
using covid19phlib.Services;
using COVID19Tracker.Library.Interfaces;
using COVID19Tracker.Xamarin.Service;
using Xamarin.Forms;

namespace COVID19Tracker.Xamarin
{
    public partial class App : Application
    {
        IIoC _ioc = null;

        public INavService Nav { get; set; } = null;

        public App()
        {
            InitializeComponent();

            this._ioc = new IoC();
            this._ioc.Reg<INavService, NavService>();

            MainPage = new NavigationPage(new MainPage());

            RegisterPages();

            //this.Nav.GoToPage(Library.Enums.Enum_NavService_Pages.RegionPage, "PH");
            //this.Nav.GoToPage(Library.Enums.Enum_NavService_Pages.CityPage, "NCR");
        }

        void RegisterPages()
        {
            // and get the instnace of this NavService
            Nav = this._ioc.GI<INavService>();
            Nav.NavPage = MainPage.Navigation;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
