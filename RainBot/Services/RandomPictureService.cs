using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RainBot.Core;

namespace RainBot.Services
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
            var apiKey = BotConfig.GetValue("GiphyToken");
            if (string.IsNullOrEmpty(apiKey))
            {
                Log.Warning("Missing Giphy API key.");
                return string.Empty;
            }

            var client = new WebClient();

            try
            {
                var api = $"https://api.giphy.com/v1/gifs/random?api_key={apiKey}&tag={tag}&rating=PG";
                var response = client.DownloadString(api);
                var json = JObject.Parse(response);
                return json.SelectToken("data").SelectToken("images").SelectToken("original").SelectToken("url").ToString();
            }
            catch (Exception ex)
            {
                Log.Error($"Error connecting to Giphy. {ex.Message}");
                return string.Empty;
            }
        }
    }
}