using System.Collections.Generic;
using System.Linq;
using RainBot.Domain;

namespace RainBot.Data
{
    public class SoundReactRepository
    {
        private BotContext _dbContext;

        public SoundReactRepository(BotContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<SoundReactEntity> GetSoundReacts()
        {
            return _dbContext.SoundReact.ToList();
        }
    }
}