using covid19phlib.Interfaces;
using covid19phlib.Services;
using COVID19Tracker.Library.Interfaces;
using COVID19Tracker.Library.Services;
using COVID19Tracker.Xamarin.Service;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace COVID19Tracker.Xamarin
{
    public partial class App : Application
    {
        IIoC _ioc = null;

        public ILogger Logger { get; set; } = null;

        public INavService Nav { get; set; } = null;

        public ISettings Settings { get; set; } = null;

        public App()
        {
            InitializeComponent();

            this._ioc = new IoC();

            this._ioc.Reg<INavService, NavService>();
            this._ioc.Reg<ISettings, Settings>();
            this._ioc.Reg<IAppCenterLogger, AppCenterLogger>();
            this._ioc.Reg<ILogger, Logger>();

            this.Logger = this._ioc.GI<ILogger>();
            this.Logger.AppCenterLogger = this._ioc.GI<IAppCenterLogger>();
            this.Logger.Start();

            this.Settings = this._ioc.GI<ISettings>();

#if DEBUG
            this.Logger.Log("Running in Debug");
#else
            this.Logger.Log("Running in Release");
#endif

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
