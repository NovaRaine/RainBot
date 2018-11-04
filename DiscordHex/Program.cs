using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using DiscordHex.Data;
using DiscordHex.Core;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using System.Net.Http;
using DiscordHex.Services;

namespace DiscordHex
{
    public class Program
    {
        private const string ConfigFile = @"c:\RainBot\Config.cfg";

        private DataLoader _loader;
        private DiscordSocketClient _client;
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Environment.SetEnvironmentVariable("Version", "3.0.0");

            var services = ConfigureServices();

            _loader = new DataLoader();
            _loader.ReadConfig(ConfigFile);
            _loader.LoadData();

            _client = services.GetRequiredService<DiscordSocketClient>();

            _client.Log += Log;
            services.GetRequiredService<CommandService>().Log += Log;
            
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("Settings_Token"));
            await _client.StartAsync();
            await _client.SetGameAsync($"RainBot-v{Environment.GetEnvironmentVariable("Version")}");
            
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()

                .AddSingleton<CommandService>()
                .AddSingleton<CommonCommands>()

                .AddSingleton<RandomPictureService>()
                .AddSingleton<SpellService>()
                .AddSingleton<FfxivSpellService>()
                .AddSingleton<SoundReactionService>()
                .AddSingleton<GameSession>()

                .BuildServiceProvider();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
