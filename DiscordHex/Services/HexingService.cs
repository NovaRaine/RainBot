using Discord;
using Discord.WebSocket;
using DiscordHex.Core;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Services
{
    public class HexingService
    {
        public EmbedBuilder CastHex(IReadOnlyCollection<IUser> users, IReadOnlyCollection<SocketRole> mentionedRoles, ulong botId)
        {
            var e = new EmbedBuilder();

            var hex = BotSettings.Instance.Hexes.ElementAt(BotSettings.Instance.RandomNumber.Next(0, BotSettings.Instance.Hexes.Count));
            if (!BotSettings.Instance.Hexes.Any())
            {
                e.Description = "I seem to have forgotten how to cast hexes.. wow :/";
                return e;
            }

            if (users != null && users.Count > 0 && users.Any(x => x.Id == botId))
            {
                e.Description = "You're targeting me? But.. but why.. what have I done :(";
                return e;
            }
            
            if (mentionedRoles.Count > 0)
            { // no mentioning roles 
                e.Description = "I'm not hexing a whole group, and you should know better.. Shame on you :(";
                return e;
            }
            
            var targets = users.Any() ? string.Join(", ", users.Select(x => x.Username)) : string.Empty;

            if (string.IsNullOrEmpty(targets))
            {
                e.Description = "I curse you all! Foolish mortals.";
                return e;
            }
            else
            {
                e.Description = $"{targets}! I cast {hex.Key} on you!";
                e.ImageUrl = string.IsNullOrEmpty(hex.Value) ? "https://cdn.awwni.me/nltc.jpg" : hex.Value;
                return e;
            }
        }
    }
}
