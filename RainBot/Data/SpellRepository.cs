using System.Collections.Generic;
using System.Linq;
using RainBot.Domain;

namespace RainBot.Data
{
    public class SpellRepository
    {
        private SpellsContext _context;

        public SpellRepository(SpellsContext context)
        {
            _context = context;
        }

        public List<SpellEntity> GetSpells()
        {
            return _context.Spells.ToList();
        }
    }
}