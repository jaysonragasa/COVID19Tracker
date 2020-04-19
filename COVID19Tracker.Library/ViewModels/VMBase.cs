using COVID19Tracker.Library.APIClient.Interfaces;
using GalaSoft.MvvmLight;

namespace covid19phlib.ViewModels
{
	public class VMBase : ViewModelBase
    {
		public IAPILocator API { get; set; } = null;

		private bool _IsLoading = false;
		public bool IsLoading
		{
			get { return _IsLoading; }
			set { Set(nameof(IsLoading), ref _IsLoading, value); }
		}
	}
}
