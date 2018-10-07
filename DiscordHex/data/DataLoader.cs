using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Discord.WebSocket;
using DiscordHex.core;

namespace DiscordHex.Data
{
    internal class DataLoader
    {    
        private string Token { get; set; }
        private string Delimiter { get; set; } = ">>";

        private const string GET_WITCHES = "select * from authorizedWitches";
        private const string GET_HEXES = "select * from spells";
        private const string GET_SETTINGS = "select * from settings";
        
        internal void LoadData()
        {
            var con = new SQLiteConnection("Data Source=c:\\temp\\hexer\\discordHexer.db;Version=3;");
            con.Open();

            // Withces
            BotSettings.Instance.ApprovedWitches = GetAuthorizedWitches(con);

            // Hexes
            BotSettings.Instance.Hexes = GetSpellBook(con);

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
            GetLiveSettings(ref botSettings, settings);
#endif

            BotSettings.Instance.Prefix = settings["delimiter"];
            var allowAll = settings["allowall"];
            BotSettings.Instance.AllowAll = allowAll.ToLower() == "true";
        }

        private void GetDebugSettings(Dictionary<string, string> settings)
        {
            ulong.TryParse((string)settings["debugid"], out var res);
            if (res > 0)
                BotSettings.Instance.SelfId = res;

            BotSettings.Instance.Token = settings["tokenDebug"];
        }

        private void GetLiveSettings(Dictionary<string, string> settings)
        {
            ulong.TryParse((string)settings["prodid"], out var res);
            if (res > 0)
                BotSettings.Instance.SelfId = res;

            BotSettings.Instance.Token = settings["token"];
        }

        private Dictionary<string, string> GetSpellBook(SQLiteConnection con)
        {
            var hexes = new Dictionary<string, string>();
            var getHexesCmd = new SQLiteCommand(GET_HEXES, con);
            var hexReader = getHexesCmd.ExecuteReader();

            while (hexReader.Read())
            {
                try
                {
                    hexes.Add((string) hexReader["name"], (string) hexReader["img"]);
                }
                catch (Exception ex)
                {
                    // just ignore
                }

            }

            hexReader.Close();
            return hexes;
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

        public void SaveUser(SocketMessage message)
        {
            var insert = "insert or ignore into userlog (id, name) VALUES (?, ?)";
            var con = new SQLiteConnection("Data Source=c:\\temp\\hexer\\discordHexer.db;Version=3;");
            
            var user = new SQLiteParameter();
            var id = new SQLiteParameter();

            user.Value = message.Author.Username;
            id.Value = message.Author.Id.ToString();

            var cmd = new SQLiteCommand {CommandText = insert, Connection = con};
            cmd.Parameters.Add(id);
            cmd.Parameters.Add(user);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
