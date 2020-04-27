using System;
using System.Collections.Generic;

namespace COVID19Tracker.Library.Interfaces
{
    public interface IAppCenterLogger
    {
        void TrackEvent(string name, IDictionary<string, string> properties = null);
        void TrackError(Exception exception, IDictionary<string, string> properties = null);
    }
}
