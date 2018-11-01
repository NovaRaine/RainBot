using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;
using System;

namespace DiscordHex.Services
{
    public class RandomPictureService
    {
        private readonly HttpClient _http;

        public RandomPictureService(HttpClient http)
            => _http = http;

        public async Task<Stream> GetPictureAsync(string url)
        {
            var resp = await _http.GetAsync(url);
            return await resp.Content.ReadAsStreamAsync();
        }

        public string GetRandomGiphyByTag(string tag)
        {
            var apiKey = Environment.GetEnvironmentVariable("Settings_GiphyToken");
            if (string.IsNullOrEmpty(apiKey))
                return string.Empty;

            var client = new WebClient();

            try
            {
                var api = $"https://api.giphy.com/v1/gifs/random?api_key={apiKey}&tag={tag}&rating=PG";
                var response = client.DownloadString(api);
                var json = JObject.Parse(response);
                return json.SelectToken("data").SelectToken("images").SelectToken("original").SelectToken("url").ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
