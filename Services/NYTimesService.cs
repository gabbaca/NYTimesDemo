using Newtonsoft.Json;
using NYTimesDemo.Helpers;
using NYTimesDemo.Models;
using NYTimesDemo.Services.Interfaces;
using System.Text.Json;

namespace NYTimesDemo.Services
{
    public class NYTimesService : INYTimesService
    {
        private IHttpClientFactory ClientFactory { get; set; }
        private readonly IConfiguration Config;

        public NYTimesService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            ClientFactory = httpClientFactory;
            Config = config;
        }

        public async Task<NYTimesArticlesModel> GetTopRecentArticles(NYTimesPeriodEnum period)
        {
            int timePeriod = (int)period;

            string apiKey = Config["NYTimesApiKey"];
            string apiEndpoint = Config["NYTimesApiEndpoint"];

            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiEndpoint}{timePeriod}.json?api-key={apiKey}");
            var client = ClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                using var responseStream = await response.Content.ReadAsStreamAsync();
                using (var sr = new StreamReader(responseStream))
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    var jsObj = serializer.Deserialize(jsonTextReader);
                    if (jsObj != null)
                    {
                        var data = NYTimesArticlesModel.FromJson(jsObj.ToString());
                        return data;
                    }
                }
            }
            return null;
        }
    }
}
