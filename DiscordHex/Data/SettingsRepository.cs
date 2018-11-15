using Dapper;
using DiscordHex.Core;
using DiscordHex.Domain;
using Npgsql;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class SettingsRepository
    {
        public List<SettingEntity> GetSpettings()
        {
            IEnumerable<SettingEntity> data = null;
            using (var conn = new NpgsqlConnection(BotSettings.Instance.Config.ConnectionString))
            {
                conn.Open();
                data = conn.Query<SettingEntity>(@"SELECT * FROM ""RainBot"".""Settings""");
            }

            return data?.ToList();
        }
    }
}
