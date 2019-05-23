using System.Collections.Generic;
using System.Linq;
using RainBot.Domain;

namespace RainBot.Data
{
    public class SoundReactRepository
    {
        private readonly RainBotContext _context;

        public SoundReactRepository(RainBotContext dbContext)
        {
            _context = dbContext;
        }

        public List<SoundReactEntity> GetSoundReacts()
        {
            return _context.SoundReact.ToList();
        }
    }
}