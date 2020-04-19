using covid19phlib.APIClient;
using covid19phlib.Interfaces;
using covid19phlib.Services;
using COVID19Tracker.Library.APIClient.Interfaces;

namespace COVID19Tracker.Library.APIClient.DataSources.MyGitRepo
{
    public abstract class APILocatorBase : IAPILocator
    {
        IWebClientService _webClientService;
        public ICountryData Country { get; set; } = null;

        public virtual void Initialize()
        {
            _webClientService = new WebClientService();
            this.Country = new CountryData(this._webClientService);
        }
    }
}
