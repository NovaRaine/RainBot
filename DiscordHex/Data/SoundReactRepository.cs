using Dapper;
using DiscordHex.Core;
using DiscordHex.Domain;
using Npgsql;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class SoundReactRepository
    {
        public List<SoundReactEntity> GetSoundReacts()
        {
            IEnumerable<SoundReactEntity> data = null;
            using (var conn = new NpgsqlConnection(BotSettings.Instance.Config.ConnectionString))
            {
                conn.Open();
                data = conn.Query<SoundReactEntity>(@"SELECT * FROM ""RainBot"".""SoundReacts""");
            }

            return data?.ToList();
        }
    }
}
