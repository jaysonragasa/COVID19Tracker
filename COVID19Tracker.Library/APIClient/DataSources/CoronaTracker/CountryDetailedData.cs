using covid19phlib.DTO_Models;
using covid19phlib.Interfaces;
using COVID19Tracker.Library.APIClient.Interfaces;
using COVID19Tracker.Library.DTO_Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVID19Tracker.Library.APIClient.DataSources.CoronaTracker
{
    public class CountryDetailedData : DataSourceBase, ICountryDetailedData
    {
        string jsondata = string.Empty;

        #region so cache them up
        List<DTO_Model_CaseInfo> JSONData = new List<DTO_Model_CaseInfo>();
        List<DTO_Model_CaseInfo> _cache_regions = new List<DTO_Model_CaseInfo>();
        List<DTO_Model_CaseInfo> _cache_city = new List<DTO_Model_CaseInfo>();
        List<DTO_Model_CaseInfo> _cache_agegroup = new List<DTO_Model_CaseInfo>();
        List<DTO_Model_CaseInfo> _cache_gender = new List<DTO_Model_CaseInfo>();
        List<DTO_Model_CaseInfo> _cache_admitted = new List<DTO_Model_CaseInfo>();
        List<DTO_Model_CaseInfo> _cache_discharged = new List<DTO_Model_CaseInfo>();
        #endregion

        // for filtering purposes
        public List<string> Regions { get; set; } = new List<string>();
        public List<string> City { get; set; } = new List<string>();
        public List<string> AgeGroup { get; set; } = new List<string>();
        public List<string> Gender { get; set; } = new List<string>();
        public List<string> Admitted { get; set; } = new List<string>();

        public CountryDetailedData(IWebClientService webClientService)
        {
            this.Web = webClientService;
        }

        public async Task<ResponseData> GetDataByCountryCode(string countryCode)
        {
            ResponseData responseData = new ResponseData();

            this.JSONData.Clear();

            if (countryCode == "PH")
            {
                JSONData = await this.Web.GetAsync<List<DTO_Model_CaseInfo>>("https://raw.githubusercontent.com/jaysonragasa/COVID19DataDrop/master/regioncitydata.json");

                this.Regions = JSONData.Select(x => x.RegionRes).Distinct().ToList();
                this.City = JSONData.Select(x => x.ProvCityRes).Distinct().ToList();
                this.AgeGroup = JSONData.Select(x => x.AgeGroup).Distinct().ToList();
                this.Gender = JSONData.Select(x => x.Sex).Distinct().ToList();
                this.Admitted = JSONData.Select(x => x.Admitted).Distinct().ToList();

                responseData.Status = true;
                responseData.Message = "GetDataByCountryCode";
            }

            return responseData;
        }

        public async Task<ResponseData> GetAllRegionsAsync()
        {
            ResponseData responseData = new ResponseData();
            List<DTO_Model_Region> caseinfolist = new List<DTO_Model_Region>();

            for (int i = 0; i < this.Regions.Count; i++)
            {
                var currentRegion = this.JSONData.Where(x => x.RegionRes == this.Regions[i]).ToList();

                // get confirmed case
                DTO_Model_Region caseInfo = new DTO_Model_Region()
                {
                    RegionName = this.Regions[i],
                    Confirmed = currentRegion.Count
                };

                var recoveredList = currentRegion.Where(x => x.RemovalType.ToUpperInvariant() == "RECOVERED").ToList();
                var deceasedList = currentRegion.Where(x => x.RemovalType.ToUpperInvariant() == "DIED").ToList();

                caseInfo.Recovered = recoveredList.Count;
                caseInfo.Deceased = deceasedList.Count;

                caseinfolist.Add(caseInfo);
            }

            responseData.Result = caseinfolist;
            responseData.Status = true;
            responseData.Message = "GetByRegionAsync";

            return responseData;
        }

        public async Task<ResponseData> GetAllCitiesAsync()
        {
            ResponseData responseData = new ResponseData();
            List<DTO_Model_City> caseinfolist = new List<DTO_Model_City>();

            for (int i = 0; i < this.City.Count; i++)
            {
                var currentCity = this._cache_regions.Where(x => x.ProvCityRes == this.City[i]).ToList();

                // get confirmed case
                DTO_Model_City caseInfo = new DTO_Model_City()
                {
                    CityName = this.City[i],
                    Confirmed = currentCity.Count
                };

                var recoveredList = currentCity.Where(x => x.RemovalType.ToUpperInvariant() == "RECOVERED").ToList();
                var deceasedList = currentCity.Where(x => x.RemovalType.ToUpperInvariant() == "DIED").ToList();

                caseInfo.Recovered = recoveredList.Count;
                caseInfo.Deceased = deceasedList.Count;

                caseinfolist.Add(caseInfo);
            }

            responseData.Result = caseinfolist;
            responseData.Status = true;
            responseData.Message = "GetByRegionAsync";

            return responseData;
        }

        public async Task<ResponseData> GetCitiesByRegionNameAsync(string regionName)
        {
            ResponseData responseData = new ResponseData();
            List<DTO_Model_City> caseinfolist = new List<DTO_Model_City>();

            this._cache_city = this.JSONData.Where(x => x.RegionRes == regionName).ToList();
            var cityNames = this._cache_city.Select(x => x.ProvCityRes).Distinct().ToList();

            for (int i = 0; i < cityNames.Count; i++)
            {
                var currentCity = this._cache_city.Where(x => x.ProvCityRes == cityNames[i]).ToList();

                // get confirmed case
                DTO_Model_City caseInfo = new DTO_Model_City()
                {
                    CityName = cityNames[i],
                    Confirmed = currentCity.Count
                };

                var recoveredList = currentCity.Where(x => x.RemovalType.ToUpperInvariant() == "RECOVERED").ToList();
                var deceasedList = currentCity.Where(x => x.RemovalType.ToUpperInvariant() == "DIED").ToList();

                caseInfo.Recovered = recoveredList.Count;
                caseInfo.Deceased = deceasedList.Count;

                caseinfolist.Add(caseInfo);
            }

            responseData.Result = caseinfolist;
            responseData.Status = true;
            responseData.Message = "GetByCityAsync";

            return responseData;
        }

        public async Task<ResponseData> GetDataByCityNameAsync(string cityName)
        {
            ResponseData responseData = new ResponseData();

            var currentCity = this._cache_city.Where(x => x.ProvCityRes == cityName).ToList();

            DTO_Model_City caseInfo = new DTO_Model_City()
            {
                CityName = cityName,
                Confirmed = currentCity.Count
            };

            var recoveredList = currentCity.Where(x => x.RemovalType.ToUpperInvariant() == "RECOVERED").ToList();
            var deceasedList = currentCity.Where(x => x.RemovalType.ToUpperInvariant() == "DIED").ToList();

            caseInfo.Recovered = recoveredList.Count;
            caseInfo.Deceased = deceasedList.Count;

            //responseData.Result = caseinfolist;
            responseData.Status = true;
            responseData.Message = "GetDataByCity";

            return responseData;
        }
    }
}
