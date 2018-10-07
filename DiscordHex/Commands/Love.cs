using System.Threading.Tasks;
using Discord.WebSocket;
using System.Linq;
using Discord;

namespace DiscordHex.Commands
{
    internal class Love : ICommand
    {
        public string Description => "Show someone you love them!";

        public async Task Execute(string[] tokens, SocketMessage message)
        {
            if (message.MentionedUsers.Count == 0)
            {
                await message.Channel.SendMessageAsync($"{message.Author.Username} seems to love.. everyone! How nice :)");
                return;
            }

            var e = new EmbedBuilder();

            // user specific ones
            switch (message.MentionedUsers.First().Id)
            {
                case 122444132156309506:
                    e.ImageUrl = "https://cdn.weeb.sh/images/r17lwymiZ.gif";
                    e.Description = $"Feel the love damnit!";
                    break;
                case 174518951085211648:
                    e.Description = $"{message.MentionedUsers.First().Username} Ultimate cuddlepuff! you got loved by {message.Author.Username}";
                    e.ImageUrl = "https://cdn.weeb.sh/images/SJYxIUmD-.gif";
                    break;
                case 271120668450357258:
                    e.ImageUrl = "https://cdn.weeb.sh/images/By03IkXsZ.gif";
                    e.Description = $"{message.MentionedUsers.First().Username} is the cutest smol bean! Everyone loves you <3";
                    break;

                default:
                    e.ImageUrl = "https://cdn.weeb.sh/images/HkzArUmvZ.gif";
                    e.Description = $"{message.MentionedUsers.First().Username}!! {message.Author.Username} loves you <3";
                    break;
            }

            await message.Channel.SendMessageAsync("", false, e);
            return;
        }
    }
}
