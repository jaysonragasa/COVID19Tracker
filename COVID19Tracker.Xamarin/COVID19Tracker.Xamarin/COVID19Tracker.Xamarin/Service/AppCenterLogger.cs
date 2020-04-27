using COVID19Tracker.Library.Interfaces;
using System;
using System.Collections.Generic;

namespace COVID19Tracker.Xamarin.Service
{
    public class AppCenterLogger : IAppCenterLogger
    {
        public void TrackError(Exception exception, IDictionary<string, string> properties = null)
        {
            Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, properties);
        }

        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(name, properties);
        }
    }
}
