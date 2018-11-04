using Dapper;
using DiscordHex.Core;
using DiscordHex.Domain;
using Npgsql;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class SpellRepository
    {
        public List<SpellEntity> GetSpells()
        {
            IEnumerable<SpellEntity> data = null;
            using (var conn = new NpgsqlConnection(BotSettings.Instance.Config.ConnectionString))
            {
                conn.Open();
                data = conn.Query<SpellEntity>(@"SELECT * FROM ""RainBot"".""Spells""");
            }

            if (data == null) return null;
            return data.ToList();
        }
    }
}
