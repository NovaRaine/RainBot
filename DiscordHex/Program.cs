using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using DiscordHex.Data;
using DiscordHex.Core;

namespace DiscordHex
{

    public class Program
    {
        private DataLoader _loader;
        private CommandHandler _commandHandler = new CommandHandler();

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            LoadData();
            
            var client = new DiscordSocketClient();
            
            client.Log += Log;
            client.MessageReceived += MessageReceived;
            
            await client.LoginAsync(TokenType.Bot, BotSettings.Instance.Token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private void LoadData()
        {
            _loader = new DataLoader();
            _loader.LoadData();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content.StartsWith((BotSettings.Instance.Prefix)))
            {
                var tokens = message.Content.Substring((BotSettings.Instance.Prefix.Length)).Split(' ');
                if (tokens.Any())
                {
                    await _commandHandler.ExecuteCommand(tokens, message);
                }
            }
        }
    }
}
