using DiscordHex.Domain;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class SpellRepository
    {
        public List<SpellEntity> GetSpells()
        {
            using (var db = new BotContext())
            {
                return db.Spells.ToList();
            }
        }
    }
}
