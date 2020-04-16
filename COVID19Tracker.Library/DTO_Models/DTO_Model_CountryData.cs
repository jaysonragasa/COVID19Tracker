using System;

namespace covid19phlib.DTO_Models
{
    public class DTO_Model_CountryData
    {
        public string countryCode { get; set; }
        public string country { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
        public int totalConfirmed { get; set; }
        public int totalDeaths { get; set; }
        public int totalRecovered { get; set; }
        public int dailyConfirmed { get; set; }
        public int dailyDeaths { get; set; }
        public int activeCases { get; set; }
        public int totalCritical { get; set; }
        public int totalConfirmedPerMillionPopulation { get; set; }
        public int? totalDeathsPerMillionPopulation { get; set; }
        public string FR { get; set; }
        public string PR { get; set; }
        public DateTime lastUpdated { get; set; }
    }
}
