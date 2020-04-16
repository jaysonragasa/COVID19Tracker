using covid19phlib.APIClient;
using covid19phlib.Interfaces;
using covid19phlib.Services;

namespace covid19phlib.ViewModels
{
    public class ViewModelLocator
    {
        IIoC _ioc;

        public ViewModelLocator()
        {
            _ioc = new IoC();

            _ioc.Reg<APILocator>();
            _ioc.Reg<ViewModel_Dashboard>(() => new ViewModel_Dashboard(this._ioc));
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