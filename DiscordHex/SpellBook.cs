using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordHex
{
    internal class SpellBook
    {
        private static readonly Random Rnd = new Random();

        private Dictionary<string, string> _hexes = new Dictionary<string, string>();

        public SpellBook(Dictionary<string, string> hexes)
        {
            _hexes = hexes;
        }

        internal async Task CastHex(SocketMessage message, bool backfire)
        {
            if (backfire) await Backfire(message);
            else await Hex(message);

        }

        private async Task Backfire(SocketMessage message)
        {
            await message.Channel.SendMessageAsync("No way! I only answer to the great Dragon Mom and her trusted coven of witches.");
        }

        private async Task Hex(SocketMessage message)
        {
            if (!_hexes.Any()) return;

            var hex = _hexes.ElementAt(Rnd.Next(0, _hexes.Count));
            var target = GetTarget(message.MentionedUsers);
            if (string.IsNullOrEmpty(target))
                await message.Channel.SendMessageAsync("I curse you all! Foolish mortals.");
            else
                await message.Channel.SendFileAsync(string.IsNullOrEmpty(hex.Value) ? "c:\\temp\\hexer\\spell.gif" : hex.Value , $"{target}! I cast {hex.Key} on you!");
        }

        private string GetTarget(IReadOnlyCollection<SocketUser> users)
        {
            var s = new StringBuilder();
            foreach (var user in users)
            {
                s.Append(user.Username);
            }

            return s.ToString();
        }
    }
}
