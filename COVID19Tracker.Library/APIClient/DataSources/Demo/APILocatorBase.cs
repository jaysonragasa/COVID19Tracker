﻿using COVID19Tracker.Library.APIClient.Interfaces;

namespace COVID19Tracker.Library.APIClient.DataSources.Demo
{
    public abstract class APILocatorBase : IAPILocator
    {
        public ICountryDetailedData CountryDetailedData { get; set; } = null;
        public ICountryData Country { get; set; } = null;

        public virtual void Initialize()
        {
            this.Country = new CountryData();
            this.CountryDetailedData = new CountryDetailedData();
        }
    }
}
