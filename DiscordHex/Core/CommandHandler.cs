using Discord.WebSocket;
using DiscordHex.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordHex.Core
{
    internal class CommandHandler
    {
        private Dictionary<string, ICommand> _commands;

        internal CommandHandler()
        {
            _commands = new Dictionary<string, ICommand>()
            {
                { "hex", new Hex() },
                { "love", new Love() },
                { "witchylove", new Love() },
                { "help", new Help() },
                { "esuna", new Esuna() }
            };

            BotSettings.Instance.CommandHelpTexts = new Dictionary<string, string>();
            foreach (var item in _commands.Keys)
            {
                BotSettings.Instance.CommandHelpTexts.Add(item, _commands[item].Description);
            }
        }

        internal async Task ExecuteCommand(string[] tokens, SocketMessage message)
        {
            _commands.TryGetValue(tokens.FirstOrDefault().ToLower().Trim(), out var command);
            if (command != null)
            {
                await command.Execute(tokens, message);
            }
        }
    }
}
