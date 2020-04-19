using COVID19Tracker.Library.Enums;
using System;

namespace COVID19Tracker.Library.Interfaces
{
    public interface INavService
    {
        object NavPage { get; set; }
        void RegNavRoute(Enum_NavService_Pages routeName, Type pageType);
        void GoToPage(Enum_NavService_Pages routeName);
        void GoBack();
    }
}
