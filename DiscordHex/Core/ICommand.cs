using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordHex.Core
{
    internal interface ICommand
    {
        Task Execute(string[] tokens, SocketMessage message);
        string Description { get; }
    }
}
