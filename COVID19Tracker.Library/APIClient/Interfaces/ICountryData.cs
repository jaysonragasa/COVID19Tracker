using covid19phlib.DTO_Models;
using System.Threading.Tasks;

namespace COVID19Tracker.Library.APIClient.Interfaces
{
    public interface ICountryData
    {
        Task<ResponseData> GetGlobal();
        Task<ResponseData> GetASEAN();
    }
}
