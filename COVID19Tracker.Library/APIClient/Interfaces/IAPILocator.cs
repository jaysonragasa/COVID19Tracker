namespace COVID19Tracker.Library.APIClient.Interfaces
{
    public interface IAPILocator
    {
        ICountryData Country { get; set; }
        ICountryDetailedData CountryDetailedData { get; set; }
    }
}
