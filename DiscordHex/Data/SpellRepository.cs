using DiscordHex.Domain;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class SpellRepository
    {
        private BotContext _dbContext;
        public SpellRepository(BotContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<SpellEntity> GetSpells()
        {
            return _dbContext.Spells.ToList();
        }
    }
}
