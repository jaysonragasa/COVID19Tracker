using System.Threading;
using System.Threading.Tasks;

namespace covid19phlib.Interfaces
{
    public interface IWebClientService
    {
        CancellationTokenSource CancelationToken { get; set; }

        Task<T> GetAsync<T>(string urlPath);
        Task<string> GetStringAsync(string urlPath);
    }
}