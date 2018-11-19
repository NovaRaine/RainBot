using DiscordHex.Domain;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
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
