using RainBot.Domain;
using System.Collections.Generic;
using System.Linq;

namespace RainBot.Data
{
    public class GuildConfigRepository
    {
        private readonly RainBotContext _context;

        public GuildConfigRepository(RainBotContext context)
        {
            _context = context;
        }

        public List<GuildConfigEntity> GetConfig(ulong guildId)
        {
            return _context.GuildConfigs.Where(x => x.GuildId == guildId.ToString()).ToList();
        }

        public void SetConfig(string key, string value, ulong guildId)
        {
            var current = _context.GuildConfigs.Where(x => x.GuildId == guildId.ToString()).FirstOrDefault();

            if (current == null)
            {
                _context.GuildConfigs.Add(new GuildConfigEntity { Key = key, Value = value, GuildId = guildId.ToString() });
            }
            else
            {
                current.Value = value;
            }

            _context.SaveChangesAsync();
        }
    }
}
