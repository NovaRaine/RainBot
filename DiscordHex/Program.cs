using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordHex
{

    public class Program
    {
        private DataLoader _loader;

        private List<ulong> _approvedWitches = new List<ulong>();
        private SpellBook _spellBook;
        private string Token { get; set; }
        private string Delimiter { get; set; } = ">>";

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            LoadData();
            
            var client = new DiscordSocketClient();
            
            client.Log += Log;
            client.MessageReceived += MessageReceived;
            
            await client.LoginAsync(TokenType.Bot, Token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private void LoadData()
        {
            _loader = new DataLoader();
            _loader.LoadData();
            Token = _loader.BotSettings.Token;
            Delimiter = _loader.BotSettings.Delimiter;
            _spellBook = _spellBook = new SpellBook(_loader.Hexes);
            _approvedWitches = _loader.AuthorizedWitches;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            SaveUser(message);
            var authorizedWitch = _approvedWitches.Contains(message.Author.Id);

            if (authorizedWitch && message.Content.ToLower() == $"{Delimiter}reloaddata")
            {
                LoadData();
                await message.Channel.SendMessageAsync("Data reloaded.");
            }

            if (message.Content.ToLower().StartsWith($"{Delimiter}hex"))
            {
                await _spellBook.CastHex(message, !authorizedWitch);
            }
        }

        private void SaveUser(SocketMessage message)
        {
            _loader.SaveUser(message);
        }
    }
}
