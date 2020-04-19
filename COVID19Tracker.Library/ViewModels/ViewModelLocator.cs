using covid19phlib.APIClient;
using covid19phlib.Interfaces;
using covid19phlib.Services;
using COVID19Tracker.Library.APIClient.Interfaces;

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
        }

        public ViewModel_Dashboard Dashboard
        {
            get
            {
                return _ioc.GI<ViewModel_Dashboard>();
            }
        }
    }
}