using covid19phlib.DTO_Models;
using covid19phlib.Interfaces;
using COVID19Tracker.Library.APIClient.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVID19Tracker.Library.APIClient.DataSources.CoronaTracker
{
    public class CountryData : CountryDataBase, ICountryData
    {
        public CountryData(IWebClientService webClientService)
        {
            this.Web = webClientService;
        }

        public async Task<ResponseData> GetGlobal()
        {
            ResponseData ret = new ResponseData();
            
            var data = await this.Web.GetAsync<List<DTO_Model_CountryData>>("https://api.coronatracker.com/v3/stats/worldometer/topCountry");

            if (data != null)
            {
                ret.Result = data;
                ret.Status = true;
                ret.Message = "GetGlobal";
            }

            return ret;
        }

        public async Task<ResponseData> GetASEAN()
        {
            ResponseData ret = new ResponseData();

            var data = await this.Web.GetAsync<List<DTO_Model_CountryData>>("https://api.coronatracker.com/v3/stats/worldometer/topCountry");

            if (data != null)
            {
                var asean_countries = data.Where(x => this.ASEANCountries.Contains(x.countryCode)).ToList();

                ret.Result = asean_countries;
                ret.Status = true;
                ret.Message = "GetASEAN";
            }

            return ret;
        }

    }
}
