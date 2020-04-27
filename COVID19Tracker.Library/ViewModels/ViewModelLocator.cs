using covid19phlib.APIClient;
using covid19phlib.Interfaces;
using covid19phlib.Services;
using COVID19Tracker.Library.APIClient.Interfaces;
using COVID19Tracker.Library.Interfaces;
using COVID19Tracker.Library.Services;
using COVID19Tracker.Library.ViewModels;

namespace covid19phlib.ViewModels
{
    public class ViewModelLocator
    {
        IIoC _ioc;
        IAPILocator _api;

        public ViewModelLocator()
        {
            _ioc = new IoC();
            _api = new APILocator();

            _ioc.Reg<APILocator>();
            _ioc.Reg<ViewModel_Dashboard>(() => new ViewModel_Dashboard(this._ioc, this._api));
            _ioc.Reg<ViewModel_Region>(() => new ViewModel_Region(this._ioc, this._api));
            _ioc.Reg<ViewModel_City>(() => new ViewModel_City(this._ioc, this._api));
            _ioc.Reg<ViewModel_CityDetailedData>(() => new ViewModel_CityDetailedData(this._ioc, this._api));
        }

        public ViewModel_Dashboard Dashboard => _ioc.GI<ViewModel_Dashboard>();
        public ViewModel_Region Region => _ioc.GI<ViewModel_Region>();
        public ViewModel_City City => _ioc.GI<ViewModel_City>();
        public ViewModel_CityDetailedData CityDetailedData => _ioc.GI<ViewModel_CityDetailedData>();
    }
}