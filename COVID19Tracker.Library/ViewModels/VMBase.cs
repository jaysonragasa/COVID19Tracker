using GalaSoft.MvvmLight;

namespace covid19phlib.ViewModels
{
	public class VMBase : ViewModelBase
    {
		private bool _IsLoading = false;
		public bool IsLoading
		{
			get { return _IsLoading; }
			set { Set(nameof(IsLoading), ref _IsLoading, value); }
		}
	}
}
