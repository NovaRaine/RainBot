using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordHex.Core;

namespace DiscordHex.Services
{
    public class OwnerService
    {
        private readonly List<string> _praise;
        private readonly List<string> _scold;

        public OwnerService()
        {
            _praise = new List<string>
            {
                "Thanks mommy! :heart:",
                "Good mommy!",
                ":smiley:"
            };

            _scold = new List<string>
            {
                "Sorry mommy :cry:",
                ":cry:",
                "Sorry, I'll try to behave!"
            };
        }
        public async Task HandleOwnerMessages(SocketCommandContext context)
        {
            var message = context.Message;
            switch (message.Content.ToLower())
            {
                case "bad bot":
                    await context.Channel.SendMessageAsync(_scold.ElementAt(Singleton.I.RandomNumber.Next(0, _scold.Count)));
                    break;
                case "good bot":
                    await context.Channel.SendMessageAsync(_praise.ElementAt(Singleton.I.RandomNumber.Next(0, _praise.Count)));
                    break;
                case "good morning rainbot":
                case "morning rainbot":
                    await context.Channel.SendMessageAsync("Good morning mommy :smiley:\nLet's have fun today!");
                    break;
            }
        }
    }
}
