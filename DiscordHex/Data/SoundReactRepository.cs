using Dapper;
using DiscordHex.Core;
using DiscordHex.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace DiscordHex.Data
{
    public class SoundReactRepository
    {
        public List<SoundReactEntity> GetSoundReacts()
        {
            IEnumerable<SoundReactEntity> data = null;
            using (var conn = new SQLiteConnection(BotSettings.Instance.ConnectionString))
            {
                conn.Open();
                data = conn.Query<SoundReactEntity>(@"SELECT * FROM soundReacts");
            }

            if (data == null) return null;
            return data.ToList();
        }
    }
}
