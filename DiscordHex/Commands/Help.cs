using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using DiscordHex.Core;
using System.Text;
using DiscordHex.Utilities;

namespace DiscordHex.Commands
{
    internal class Help : ICommand
    {
        public string Description => "Uhm.. help?";

        public async Task Execute(string[] tokens, SocketMessage message)
        {
            var helpIntro = $"Thank you for calling customer service.\nThe current prefix is: {BotSettings.Instance.Prefix}\n\nAnd here's a list of available commands:";
            var commandTexts = new StringBuilder();

            foreach(var item in BotSettings.Instance.CommandHelpTexts)
            {
                commandTexts.AppendLine($"{Helper.Pad(item.Key, " ", 25)}{item.Value}");
            }

            await message.Author.SendMessageAsync(helpIntro + $"\n```\n{commandTexts.ToString()}```");

        }
    }
}
