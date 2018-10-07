using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using DiscordHex.Data;
using DiscordHex.Commands;
using DiscordHex.core;

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
                var content = message.Content.Substring((BotSettings.Instance.Prefix.Length));
                var tokens = content.Split(' ');
                if (tokens.Any())
                {
                    await _commandHandler.ExecuteCommand(tokens, message);
                }
            }
        }

        //private async Task backupcrap(SocketMessage message)
        //{
        //    if (message.Content.ToLower().StartsWith($"{Prefix}nyanta"))
        //    {
        //        var e = new EmbedBuilder();
        //        e.ImageUrl = "https://cdn.weeb.sh/images/r17lwymiZ.gif";
        //        e.Description = $"Nyanta is the cutest smol bean!";

        //        await message.Channel.SendMessageAsync("", false, e);
        //        return;
        //    }

        //    if (message.Content.ToLower().StartsWith($"{Prefix}mionee"))
        //    {
        //        var e = new EmbedBuilder();
        //        e.ImageUrl = "https://cdn.weeb.sh/images/By03IkXsZ.gif";
        //        e.Description = $"Mionee is the cutest smol bean!";

        //        await message.Channel.SendMessageAsync("", false, e);
        //        return;
        //    }

        //    if (message.Content.ToLower().StartsWith($"{Prefix}deni"))
        //    {
        //        var e = new EmbedBuilder();
        //        e.ImageUrl = "https://cdn.weeb.sh/images/SJYxIUmD-.gif";
        //        e.Description = $"Cuddlepuff!";

        //        await message.Channel.SendMessageAsync("", false, e);
        //        return;
        //    }


        //    if (message.Content.ToLower().StartsWith($"{Prefix}atumra"))
        //    {
        //        await message.Channel.SendMessageAsync($"You silly baby..");
        //        return;
        //    }

        //    if (message.Content.ToLower().StartsWith($"{Prefix}hex"))
        //    {
                
        //    }
        //}
    }
}
