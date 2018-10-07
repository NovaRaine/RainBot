using System.Threading.Tasks;
using Discord.WebSocket;
using System.Linq;
using DiscordHex.core;

namespace DiscordHex.Commands
{
    internal class Hex : ICommand
    {
        public async Task Execute(string[] tokens, SocketMessage message)
        {
            if (message.MentionedUsers.Any(x => x.Id == BotSettings.Instance.SelfId))
            {
                await message.Channel.SendMessageAsync("You're targeting me? But.. but why.. what have I done :(");
                return;
            }

            await CastHex(message);
        }

        private async Task CastHex(SocketMessage message)
        {
            if (!BotSettings.Instance.Hexes.Any()) return; // No entries in db or something went wrong when reading hexes
            if (message.MentionedRoles.Count > 0)
            { // no mentioning roles 
                await message.Channel.SendMessageAsync("I'm not hexing a whole group, and you should know better.. Shame on you :(");
                return;
            }

            var hex = BotSettings.Instance.Hexes.ElementAt(BotSettings.Instance.RandomNumber.Next(0, BotSettings.Instance.Hexes.Count));
            var targets = message.MentionedUsers.Any() ? string.Join(", ", message.MentionedUsers.Select(x => x.Username)) : string.Empty;

            if (string.IsNullOrEmpty(targets))
            {
                await message.Channel.SendMessageAsync("I curse you all! Foolish mortals.");
                return;
            }
            else
            {
                var e = new Discord.EmbedBuilder();
                e.ImageUrl = string.IsNullOrEmpty(hex.Value) ? "https://cdn.awwni.me/nltc.jpg" : hex.Value;
                e.Description = $"{targets}! I cast {hex.Key} on you!";

                await message.Channel.SendMessageAsync("", false, e);
            }
        }

        private bool IsAuthorized(ulong id)
        {
            if (BotSettings.Instance.AllowAll)
                return true;
            else
                return BotSettings.Instance.ApprovedWitches.Contains(id);
        }
    }
}
