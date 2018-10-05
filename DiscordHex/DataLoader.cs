using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using Discord.WebSocket;

namespace DiscordHex
{
    internal class DataLoader
    {
        #region Properties

        internal Dictionary<string, string> Hexes { get; set; }
        internal List<ulong> AuthorizedWitches { get; set; }
        internal Settings BotSettings { get; set; }

        #endregion
    
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
            AuthorizedWitches = GetAuthorizedWitches(con);

            // Hexes
            Hexes = GetSpellBook(con);

            // Settings
            BotSettings = GetSettings(con);

            con.Clone();
        }

        private Settings GetSettings(SQLiteConnection con)
        {
            var botSettings = new Settings();
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

            botSettings.Token = settings["token"];
            botSettings.Delimiter = settings["delimiter"];
            return botSettings;
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

    internal class Settings
    {
        internal string Token { get; set; }
        internal string Delimiter { get; set; }
    }
}
