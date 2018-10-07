using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordHex.Commands
{
    internal class CommandHandler
    {
        private Dictionary<string, ICommand> _commands;

        internal CommandHandler()
        {
            _commands = new Dictionary<string, ICommand>()
            {
                {"hex", new Hex() }
            };
        }

        internal async Task ExecuteCommand(string[] tokens, SocketMessage message)
        {
            _commands.TryGetValue(tokens.FirstOrDefault(), out var command);
            if (command != null)
            {
                await command.Execute(tokens, message);
            }
        }
    }
}
