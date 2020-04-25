using COVID19Tracker.Library.Interfaces;
using Xamarin.Essentials;

namespace COVID19Tracker.Xamarin.Service
{
    public class Settings : ISettings
    {
        public object GetSetting(string key, string default_value)
        {
            return Preferences.Get(key, default_value);
        }

        public void SaveSettings(string key, string value)
        {
            Preferences.Set(key, value);
        }
    }
}
