using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.IO;
using Serilog;

namespace DiscordHex.Core
{
    public static class BotConfig
    {
        private static ConcurrentDictionary<string, string> _configuration = new ConcurrentDictionary<string, string>();
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer();
        private const string ConfigFile = @"c:\RainBot\Config.cfg";

        public static string GetValue(string value)
        {
            if (_configuration.IsEmpty) LoadConfig();

            _configuration.TryGetValue(value, out var res);
            return res;
        }

        private static void LoadConfig()
        {
            if (!File.Exists(ConfigFile))
            {
                Log.Fatal($"Can't find config file: '{ConfigFile}'");
                throw new IOException($"Can't find config file: '{ConfigFile}'");
            }

            using (var sr = File.OpenText(ConfigFile))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    _configuration = JsonSerializer.Deserialize<ConcurrentDictionary<string, string>>(reader);
                }
            }
        }
    }
}
