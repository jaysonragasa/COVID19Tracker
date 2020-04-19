using covid19phlib.Interfaces;

namespace COVID19Tracker.Library.APIClient.DataSources
{
    public class CountryDataBase
    {
        public IWebClientService Web { get; set; } = null;

        public string[] ASEANCountries
        {
            get
            {
                return new string[] {
                    "ID", // indonesia
                    "MY", // malaysia
                    "PH", // philippines
                    "SG", // singapore
                    "TH", // thailand
                    "BN", // brunai
                    "LA", // laos
                    "MM", // myanmar
                    "KH", // cambodia
                    "VN"  // vietname
                };
            }
        }
    }
}