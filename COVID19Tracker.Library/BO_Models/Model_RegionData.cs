using GalaSoft.MvvmLight;

namespace COVID19Tracker.Library.BO_Models
{
    public class Model_RegionData : ViewModelBase
    {
		public string RegionCode { get; set; } = string.Empty;

        private string _RegionName = string.Empty;
        public string RegionName
        {
            get { return _RegionName; }
            set { Set(nameof(RegionName), ref _RegionName, value); }
        }

        private int _TotalConfirmed = 0;
		public int TotalConfirmed
		{
			get { return _TotalConfirmed; }
			set { Set(nameof(TotalConfirmed), ref _TotalConfirmed, value); }
		}

		private int _TotalRecovered = 0;
		public int TotalRecovered
		{
			get { return _TotalRecovered; }
			set { Set(nameof(TotalRecovered), ref _TotalRecovered, value); }
		}

		private int _TotalDeaths = 0;
		public int TotalDeaths
		{
			get { return _TotalDeaths; }
			set { Set(nameof(TotalDeaths), ref _TotalDeaths, value); }
		}
	}
}
