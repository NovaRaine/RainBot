using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Services
{
    public class CommonCommands
    {
        public EmbedBuilder LoveSomeone(IReadOnlyCollection<IUser> users, IReadOnlyCollection<SocketRole> mentionedRoles, string caster)
        {
            var e = new EmbedBuilder();
            
            if (users == null || !users.Any())
            {
                e.Description = $"{caster} seems to love.. everyone! How nice :)";
                return e;
            }

            switch (users.First().Id)
            {
                // User specific ones
                case 122444132156309506:
                    e.ImageUrl = "https://cdn.weeb.sh/images/r17lwymiZ.gif";
                    e.Description = $"Feel the love damnit!";
                    break;
                case 174518951085211648:
                    e.Description = $"{users.First().Username} Ultimate cuddlepuff! you got loved by {caster}";
                    e.ImageUrl = "https://cdn.weeb.sh/images/SJYxIUmD-.gif";
                    break;
                case 271120668450357258:
                    e.ImageUrl = "https://cdn.weeb.sh/images/By03IkXsZ.gif";
                    e.Description = $"{users.First().Username} is the cutest smol bean! Everyone loves you <3";
                    break;

                // Default = everyone else
                default:
                    e.ImageUrl = "https://cdn.weeb.sh/images/HkzArUmvZ.gif";
                    e.Description = $"{users.First().Username}!! {caster} loves you <3";
                    break;
            }

            return e;
        }
    }
}
