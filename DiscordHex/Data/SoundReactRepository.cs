using DiscordHex.Domain;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class SoundReactRepository
    {
        public List<SoundReactEntity> GetSoundReacts()
        {
            using (var db = new BotContext())
            {
                return db.SoundReact.ToList();
            }
        }
    }
}
