using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiChallenge.Helpers
{
    public class NewsApiService : IDisposable
    {
        NewsApiConfig newsApiConfig { get; set; }

        public HttpClient Client { get; private set; }

        public NewsApiService(NewsApiConfig newsApiConfig)
        {
            this.newsApiConfig = newsApiConfig;

            this.Client = new HttpClient();
            Client.BaseAddress = new Uri(newsApiConfig.BaseUri);
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory");
        }

        public async Task<NewsItem> GetDetails(int id)
        {
            var endpoint = this.newsApiConfig.EndPoints.Where(e => e.Name == "item").First();
            var uri = $"{endpoint.Uri}".Replace("{}", id.ToString());

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return (await SendRequest<NewsItem>(request));
        }

        public async Task<int> GetMaxItem()
        {
            var endpoint = this.newsApiConfig.EndPoints.Where(e => e.Name == "maxitem").First();
            var uri = $"{endpoint.Uri}";

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var s = await SendRequest<int>(request);
            return s;
        }

        private async Task<T> SendRequest<T>(HttpRequestMessage requestMessage)
        {
            var response = await this.Client.SendAsync(requestMessage);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        public void Dispose()
        {
            this.Client.Dispose();
        }
    }
}
