using COVID19Tracker.Library.Enums;
using COVID19Tracker.Library.Interfaces;
using System;
using Xamarin.Forms;

namespace COVID19Tracker.Xamarin.Service
{
    public class NavService : INavService
    {
        public object NavPage { get; set; }

        public void RegNavRoute(Enum_NavService_Pages routeName, Type pageType)
        {
            Routing.RegisterRoute(routeName.ToString(), pageType);
        }

        public void GoBack()
        {
            ((INavigation)this.NavPage).PopAsync();
        }

        public void GoToPage(Enum_NavService_Pages routeName)
        {
            switch(routeName)
            {
                case Enum_NavService_Pages.About:
                    ((INavigation)this.NavPage).PushAsync(new AboutPage());
                    break;
            }
        }
    }
}
