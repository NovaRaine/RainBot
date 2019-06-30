using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;
using RainBot.Data;

namespace RainBot.Core
{
    public static class BotConfig
    {
        private static ConcurrentDictionary<string, string> _configuration = new ConcurrentDictionary<string, string>();
        private static readonly JsonSerializer _jsonSerializer = new JsonSerializer();
#if DEBUG
        private const string ConfigFile = @"c:\RainBot\Config.cfg";
#endif
#if !DEBUG
        private const string ConfigFile = @"/etc/RainBot/config.cfg";
#endif

        public static bool IsDragonMom(ulong id)
        {
            return id == 462658205009575946;
        }

        public static string GetValue(string value)
        {
            if (_configuration.IsEmpty) LoadConfig();

            _configuration.TryGetValue(value, out var res);

            return res ?? string.Empty;
        }

        public static void SetValue(string key, string value)
        {
            if (_configuration.IsEmpty) LoadConfig();

            if (!_configuration.ContainsKey(key))
            {
                _configuration.TryAdd(key, value);
            }
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
                    _configuration = _jsonSerializer.Deserialize<ConcurrentDictionary<string, string>>(reader);
                }
            }
        }
    }
}