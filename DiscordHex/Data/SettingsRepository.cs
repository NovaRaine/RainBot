using Dapper;
using DiscordHex.Core;
using DiscordHex.Domain;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace DiscordHex.Data
{
    public class SettingsRepository
    {
        public List<SettingEntity> GetSpettings()
        {
            IEnumerable<SettingEntity> data = null;
            using (var conn = new SQLiteConnection(BotSettings.Instance.ConnectionString))
            {
                conn.Open();
                data = conn.Query<SettingEntity>(@"SELECT * FROM settings");
            }

            if (data == null) return null;
            return data.ToList();
        }
    }
}
