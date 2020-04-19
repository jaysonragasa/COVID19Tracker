using covid19phlib.Interfaces;
using covid19phlib.Services;
using COVID19Tracker.Library.Interfaces;
using COVID19Tracker.Xamarin.Service;
using GalaSoft.MvvmLight.Ioc;
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

            MainPage = new NavigationPage(new MainPage());

            RegisterPages();
        }

        void RegisterPages()
        {
            // Register our Navigtaion interface to use NavService class
            this._ioc.Reg<INavService, NavService>();

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
