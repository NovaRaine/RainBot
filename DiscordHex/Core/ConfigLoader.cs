using Newtonsoft.Json;
using System.IO;
using DiscordHex.Domain;

namespace DiscordHex.Core
{
    internal class ConfigLoader
    {
        internal void Load(string configFile)
        {
            using (StreamReader file = File.OpenText(configFile))
            {
                var config = JsonConvert.DeserializeObject<BotConfigurationEntity>(file.ReadToEnd());
                BotSettings.Instance.Config = config;
            }
        }
    }
}
