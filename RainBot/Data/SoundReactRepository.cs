using System.Collections.Generic;
using System.Linq;
using RainBot.Domain;

namespace RainBot.Data
{
    public class SoundReactRepository
    {
        private SoundReactContext _context;

        public SoundReactRepository(SoundReactContext dbContext)
        {
            _context = dbContext;
        }

        public List<SoundReactEntity> GetSoundReacts()
        {
            return _context.SoundReact.ToList();
        }
    }
}