using covid19phlib.Interfaces;
using covid19phlib.Services;

namespace covid19phlib.APIClient
{
    public class APILocator
    {
        IIoC _ioc;
        IWebClientService _webClientService;

        public CountryData Country
        {
            get { return this._ioc.GI<CountryData>(); }
        }

        public APILocator()
        {
            _webClientService = new WebClientService();
            this._ioc = new IoC();

            this._ioc.Reg<CountryData>(() => new CountryData(this._webClientService));
        }
    }
}
