using DiscordHex.Core;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;

namespace DiscordHex.Commands
{
    internal class Esuna : ICommand
    {
        public string Description => "Cast the legendary spell Esuna on yourself or a target!";

        public async Task Execute(string[] tokens, SocketMessage message)
        {
            // https://ffxiv.consolegameswiki.com/mediawiki/images/3/39/Esuna.png

            if (message.MentionedRoles.Count > 0)
            { // no mentioning roles 
                await message.Channel.SendMessageAsync("Don't tag groups! Noone likese that..");
                return;
            }

            var target = message.MentionedUsers.Any() ? message.MentionedUsers.First().Username : string.Empty;

            if (string.IsNullOrEmpty(target))
            {
                var e = new Discord.EmbedBuilder();
                e.ImageUrl = "https://ffxiv.consolegameswiki.com/mediawiki/images/3/39/Esuna.png";
                e.Description = $"{message.Author.Username} performes an Esuna selfcast. It seems to have done something, but we're not sure what.";

                await message.Channel.SendMessageAsync("", false, e);
                return;
            }
            else
            {
                var e = new Discord.EmbedBuilder();
                e.ImageUrl = "https://ffxiv.consolegameswiki.com/mediawiki/images/3/39/Esuna.png";
                e.Description = $"{message.Author.Username} casts Esuna on {target}. It took 3 minutes, and had some visual effect. Noone knows if it actually worked.";

                await message.Channel.SendMessageAsync("", false, e);
            }
        }
    }
}
