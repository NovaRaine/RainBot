using Dapper;
using DiscordHex.Core;
using DiscordHex.Domain;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace DiscordHex.Data
{
    public class SpellRepository
    {
        public List<SpellEntity> GetSpells()
        {
            IEnumerable<SpellEntity> data = null;
            using (var conn = new SQLiteConnection(BotSettings.Instance.ConnectionString))
            {
                conn.Open();
                data = conn.Query<SpellEntity>(@"SELECT * FROM spells");
            }

            if (data == null) return null;
            return data.ToList();
        }
    }
}
