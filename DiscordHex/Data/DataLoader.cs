using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using DiscordHex.Domain;
using DiscordHex.Core;

namespace DiscordHex.Data
{
    internal class DataLoader
    {    
        private readonly SettingsRepository _settingsRepository;

        public DataLoader()
        {
            _settingsRepository = new SettingsRepository();
        }
        
        internal void LoadData()
        {
            var settings = _settingsRepository.GetSpettings();

#if DEBUG
            Environment.SetEnvironmentVariable("Settings_Token", settings.FirstOrDefault(x => x.Name == "tokenDebug")?.Value);
#endif
#if !DEBUG
            Environment.SetEnvironmentVariable("Settings_Token", settings.FirstOrDefault(x => x.Name == "token")?.Value);
#endif

            Environment.SetEnvironmentVariable("Settings_Prefix", settings.FirstOrDefault(x => x.Name == "prefix")?.Value);
            Environment.SetEnvironmentVariable("Settings_GiphyToken", settings.FirstOrDefault(x => x.Name == "giphyToken")?.Value);
        }

        internal void ReadConfig(string configFile)
        {
            using (StreamReader file = File.OpenText(configFile))
            {
                var config = JsonConvert.DeserializeObject<BotConfigurationEntity>(file.ReadToEnd());
                BotSettings.Instance.Config = config;
            }
        }
    }
}
