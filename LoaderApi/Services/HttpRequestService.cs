using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoaderApi.Services
{
    public class HttpRequestService
    {
        private readonly IHttpClientFactory _client;

        public HttpRequestService(IHttpClientFactory client)       
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> UpdateAggregator()
        {
            Console.WriteLine("Updating the Aggregator API");
            string uri = "https://localhost:5008/api/Aggregator/Aggregate";
            var httpClient = _client.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            return response;
        }
        
    }
}
