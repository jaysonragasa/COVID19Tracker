//#define TEST

using covid19phlib.DTO_Models;
using covid19phlib.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace covid19phlib.APIClient
{
    public class CountryData 
    {
        IWebClientService _webclient;

        public CountryData(IWebClientService webClientService)
        {
            this._webclient = webClientService;
        }

#if TEST
        public async Task<ResponseData> GetCountryData()
        {
            ResponseData ret = new ResponseData();

            var data = JsonConvert.DeserializeObject<List<DTO_Model_CountryData>>("[{\"countryCode\":\"US\",\"country\":\"USA\",\"lat\":37.09024,\"lng\":-95.712891,\"totalConfirmed\":588465,\"totalDeaths\":23711,\"totalRecovered\":37326,\"dailyConfirmed\":1524,\"dailyDeaths\":71,\"activeCases\":527428,\"totalCritical\":12772,\"totalConfirmedPerMillionPopulation\":1778,\"totalDeathsPerMillionPopulation\":72,\"FR\":\"4.0293\",\"PR\":\"6.3429\",\"lastUpdated\":\"2020-04-14T14:35:09.000Z\"},{\"countryCode\":\"ES\",\"country\":\"Spain\",\"lat\":40.463667,\"lng\":-3.74922,\"totalConfirmed\":172541,\"totalDeaths\":18056,\"totalRecovered\":67504,\"dailyConfirmed\":2442,\"dailyDeaths\":300,\"activeCases\":86981,\"totalCritical\":7371,\"totalConfirmedPerMillionPopulation\":3690,\"totalDeathsPerMillionPopulation\":386,\"FR\":\"10.4648\",\"PR\":\"39.1235\",\"lastUpdated\":\"2020-04-14T14:35:09.000Z\"},{\"countryCode\":\"IT\",\"country\":\"Italy\",\"lat\":41.87194,\"lng\":12.56738,\"totalConfirmed\":159516,\"totalDeaths\":20465,\"totalRecovered\":35435,\"dailyConfirmed\":0,\"dailyDeaths\":0,\"activeCases\":103616,\"totalCritical\":3260,\"totalConfirmedPerMillionPopulation\":2638,\"totalDeathsPerMillionPopulation\":338,\"FR\":\"12.8294\",\"PR\":\"22.2141\",\"lastUpdated\":\"2020-04-14T14:35:09.000Z\"}]");
            
            if (data != null)
            {
                ret.Result = data;
                ret.Status = true;
                ret.Message = "GetCountryData";
            }

            return ret;
        }
#else
        public async Task<ResponseData> GetCountryData()
        {
            ResponseData ret = new ResponseData();

            //var data = await _webclient.GetAsync<DTO_Model_Countries>("https://raw.githubusercontent.com/Nullstr1ng/Covid19Ph/master/country.json");
            var data = await _webclient.GetAsync<List<DTO_Model_CountryData>>("https://api.coronatracker.com/v3/stats/worldometer/topCountry");

            if (data != null)
            {
                ret.Result = data;
                ret.Status = true;
                ret.Message = "GetCountryData";
            }

            return ret;
        }
#endif
    }
}
