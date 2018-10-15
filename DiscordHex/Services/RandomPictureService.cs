using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;

namespace DiscordHex.Services
{
    public class RandomPictureService
    {
        private readonly HttpClient _http;
        private readonly string bunniesApi = "https://api.bunnies.io/v2/loop/random/?media=poster";

        public RandomPictureService(HttpClient http)
            => _http = http;

        public async Task<Stream> GetPictureAsync(string url)
        {
            var resp = await _http.GetAsync(url);
            return await resp.Content.ReadAsStreamAsync();
        }

        public string GetRandomBunnyGif()
        {
            var client = new WebClient();

            try
            {
                var response = client.DownloadString(bunniesApi);
                var json = JObject.Parse(response);
                return json.SelectToken("media").SelectToken("poster").ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
