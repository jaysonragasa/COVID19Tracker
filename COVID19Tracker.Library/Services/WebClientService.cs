using covid19phlib.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace covid19phlib.Services
{
    public class WebClientService : IWebClientService
    {
        HttpClient _httpClient = null;
        string _connectionKey = "";

        public CancellationTokenSource CancelationToken { get; set; } = new CancellationTokenSource();

        HttpClient ConnectClient()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient()
                {
                    //MaxResponseContentBufferSize = 1000000,
                    Timeout = TimeSpan.FromSeconds(30)
                };

                var authData = _connectionKey;
                var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                //_httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
                //_httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e3215ea2b8e04f45a8e615c405759d79");
            }
            return _httpClient;
        }

        public async Task<string> GetStringAsync(string urlPath)
        {
            var client = ConnectClient();

            string retItem = "";
            try
            {
                var response = await client.GetAsync(urlPath);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    retItem = content;
                }
                else
                {
                    //Logger.I.Log("ApiServiceController::GetItemByIdAsync " + urlPath + "/" + id + " failed: " + response.StatusCode.ToString());
                }
            }
            catch (HttpRequestException httpException)
            {
                //Logger.I.LogError(httpException);
            }
            catch (Exception ex)
            {
                //Logger.I.LogError(ex);
            }
            return retItem;
        }

        public async Task<T> GetAsync<T>(string urlPath)
        {
            var client = ConnectClient();

            try
            {
                var response = await client.GetAsync(urlPath, this.CancelationToken.Token);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    //string content = await response.Content.ReadAsStringAsync();

                    var des = DeserializeJsonFromStream<T>(await response.Content.ReadAsStreamAsync());

                    return (T)Convert.ChangeType(des, typeof(T));
                }
                else
                {
                    //Logger.I.Log("ApiServiceController::GetItemByIdAsync " + urlPath + "/" + id + " failed: " + response.StatusCode.ToString());
                    return default(T);
                }
            }
            catch (HttpRequestException httpException)
            {
                //Logger.I.LogError(httpException);
                return default(T);
            }
            catch (Exception ex)
            {
                //Logger.I.LogError(ex);
                return default(T);
            }
            return default(T);
        }

        // https://johnthiriet.com/efficient-api-calls/#
        static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }
    }
}
