using System.Collections.Generic;
using System.Linq;
using RainBot.Domain;

namespace RainBot.Data
{
    public class SpellRepository
    {
        private readonly RainBotContext _context;

        public SpellRepository(RainBotContext context)
        {
            _context = context;
        }

        public List<SpellEntity> GetSpells()
        {
            return _context.Spells.ToList();
        }
    }
}