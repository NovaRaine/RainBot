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
        private DataLoader _loader;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Environment.SetEnvironmentVariable("Version", "2.0.0");

            var services = ConfigureServices();

            LoadData();
            
            var client = services.GetRequiredService<DiscordSocketClient>();

            client.Log += Log;
            services.GetRequiredService<CommandService>().Log += Log;
            
            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("Settings_Token"));
            await client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<RandomCatPictureService>()
                .AddSingleton<HexingService>()
                .AddSingleton<FfxivSpellService>()
                .AddSingleton<CommonCommands>()
                .BuildServiceProvider();
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
    }
}
