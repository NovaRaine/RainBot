using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DiscordHex
{

    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();

            client.Log += Log;
            client.MessageReceived += MessageReceived;

            string token = ""; // Remember to keep this private!
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!dikdik")
            {
                await message.Channel.SendFileAsync("C:\\tmp\\dikdikpic.jpg", $"{message.Author.Mention} - here's a dik-dik-pic");
            }
        }
    }
}
