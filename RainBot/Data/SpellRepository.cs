using System.Collections.Generic;
using System.Linq;
using RainBot.Domain;

namespace RainBot.Data
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