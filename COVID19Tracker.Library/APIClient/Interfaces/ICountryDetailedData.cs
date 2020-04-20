using covid19phlib.DTO_Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COVID19Tracker.Library.APIClient.Interfaces
{
    public interface ICountryDetailedData
    {
        List<string> Admitted { get; set; }
        List<string> AgeGroup { get; set; }
        List<string> City { get; set; }
        List<string> Gender { get; set; }
        List<string> Regions { get; set; }

        Task<ResponseData> GetAllCitiesAsync();
        Task<ResponseData> GetAllRegionsAsync();
        Task<ResponseData> GetCitiesByRegionNameAsync(string regionName);
        Task<ResponseData> GetDataByCityNameAsync(string cityName);
        Task<ResponseData> GetDataByCountryCode(string countryCode);
    }
}