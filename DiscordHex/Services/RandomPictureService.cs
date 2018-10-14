using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

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
    }
}
