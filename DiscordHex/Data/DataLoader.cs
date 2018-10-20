using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Discord.WebSocket;
using DiscordHex.Core;
using DiscordHex.Services;
using System.Linq;

namespace DiscordHex.Data
{
    internal class DataLoader
    {    
        private const string GET_WITCHES = "select * from authorizedWitches";
        private const string GET_HEXES = "select * from spells";
        private const string GET_SETTINGS = "select * from settings";
        
        internal void LoadData()
        {
            var con = new SQLiteConnection("Data Source=c:\\temp\\hexer\\discordHexer.db;Version=3;");
            con.Open();

            // Withces
            BotSettings.Instance.AuthorizedWitches = GetAuthorizedWitches(con);

            // Hexes
            var spells = GetSpellBook(con);
            BotSettings.Instance.Hexes = spells.Where(x => x.Type == SpellType.Hex).ToList();
            BotSettings.Instance.Buffs = spells.Where(x => x.Type == SpellType.Buff).ToList();
            BotSettings.Instance.DirectDamage = spells.Where(x => x.Type == SpellType.DirectDamage).ToList();

            // Settings
            GetSettings(con);

            con.Clone();
        }

        private void GetSettings(SQLiteConnection con)
        {
            var getSettingsCmd = new SQLiteCommand(GET_SETTINGS, con);
            var settingsReader = getSettingsCmd.ExecuteReader();
            var settings = new Dictionary<string, string>();

            while (settingsReader.Read())
            {
                try
                {
                    settings.Add((string) settingsReader["name"], (string) settingsReader["value"]);
                }
                catch (Exception ex)
                {
                    // just ignore
                }
            }

            settingsReader.Close();

#if DEBUG
            GetDebugSettings(settings);
#endif
#if !DEBUG
            GetLiveSettings(settings);
#endif

            Environment.SetEnvironmentVariable("Settings_Prefix", settings["prefix"]);
            Environment.SetEnvironmentVariable("Settings_AllowAll", settings["allowall"]);
            Environment.SetEnvironmentVariable("Settings_GiphyToken", settings["giphyToken"]);
        }

        private void GetDebugSettings(Dictionary<string, string> settings)
        {
            Environment.SetEnvironmentVariable("Settings_Token", settings["tokenDebug"]);
        }

        private void GetLiveSettings(Dictionary<string, string> settings)
        {
            Environment.SetEnvironmentVariable("Settings_Token", settings["token"]);
        }

        private List<SpellEntity> GetSpellBook(SQLiteConnection con)
        {
            var spells = new List<SpellEntity>();
            var getHexesCmd = new SQLiteCommand(GET_HEXES, con);
            var hexReader = getHexesCmd.ExecuteReader();

            while (hexReader.Read())
            {
                try
                {
                    var s = new SpellEntity();
                    s.Name = (string)hexReader["name"];
                    s.ImageUrl = (string)hexReader["img"];
                    s.Type = (SpellType)(int)hexReader["type"];
                    spells.Add(s);
                }
                catch (Exception ex)
                {
                    // just ignore
                }

            }

            hexReader.Close();
            return spells;
        }

        public List<ulong> GetAuthorizedWitches(SQLiteConnection con)
        {
            var authorizedWithces = new List<ulong>();
            var getWitchesCmd = new SQLiteCommand(GET_WITCHES, con);
            var witchReader = getWitchesCmd.ExecuteReader();

            while (witchReader.Read())
            {
                ulong.TryParse((string)witchReader["discordId"], out var res);
                if (res > 0)
                    authorizedWithces.Add(res);
            }
            witchReader.Close();

            return authorizedWithces;
        }
    }
}
