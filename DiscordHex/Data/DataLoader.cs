using System;
using DiscordHex.Core;
using System.Linq;
using DiscordHex.Domain;

namespace DiscordHex.Data
{
    internal class DataLoader
    {    
        private readonly SpellRepository _spellRepository;
        private readonly SettingsRepository _settingsRepository;

        public DataLoader()
        {
            _spellRepository = new SpellRepository();
            _settingsRepository = new SettingsRepository();
        }
        
        internal void LoadData()
        {
            // Spells
            var spells = _spellRepository.GetSpells();
            BotSettings.Instance.Hexes = spells.Where(x => x.Type == SpellType.Hex).ToList();
            BotSettings.Instance.Buffs = spells.Where(x => x.Type == SpellType.Buff).ToList();
            BotSettings.Instance.DirectDamage = spells.Where(x => x.Type == SpellType.DirectDamage).ToList();

            // Settings
            GetSettings();
        }

        private void GetSettings()
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
    }
}
