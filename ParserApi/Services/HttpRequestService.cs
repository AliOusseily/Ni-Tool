using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParserApi.Services
{
    public class HttpRequestService
    {
        private readonly IHttpClientFactory _client;

        public HttpRequestService(IHttpClientFactory client)       
        {
            _client = client;
        }

        public async Task<bool> UpdateLoaderApi(string json)
        {
            string uri = "https://localhost:5004/api/Loader/Load";
            var httpClient = _client.CreateClient();
            var response = await httpClient.PostAsync(uri, new StringContent(json , Encoding.UTF8, "application/json"));
            return true;
        }
        
    }
}
