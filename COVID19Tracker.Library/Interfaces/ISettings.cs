namespace COVID19Tracker.Library.Interfaces
{
    public interface ISettings
    {
        object GetSetting(string key, string default_value);
        void SaveSettings(string key, string value);
    }
}
