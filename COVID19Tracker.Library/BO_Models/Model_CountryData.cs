﻿using GalaSoft.MvvmLight;

namespace covid19phlib.BO_Models
{
	public class Model_CountryData : ViewModelBase
    {
		private string _CountryName = string.Empty;
		public string CountryName
		{
			get { return _CountryName; }
			set { Set(nameof(CountryName), ref _CountryName, value); }
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
