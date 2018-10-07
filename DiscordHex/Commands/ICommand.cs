using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordHex.Commands
{
    internal interface ICommand
    {
        Task Execute(string[] tokens, SocketMessage message);
    }
}
