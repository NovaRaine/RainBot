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
using System.Linq;

namespace DiscordHex
{
    public class Program
    {
        private DataLoader _loader;
        private DiscordSocketClient _client;
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Environment.SetEnvironmentVariable("Version", "2.1.4");

            var services = ConfigureServices();
            
            LoadData();
            
            _client = services.GetRequiredService<DiscordSocketClient>();

            _client.Log += Log;
            services.GetRequiredService<CommandService>().Log += Log;
            
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("Settings_Token"));
            await _client.StartAsync();
            await _client.SetGameAsync($"RainBot-v{Environment.GetEnvironmentVariable("Version")}");
            
            _client.Connected += OnClientConnected;
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnClientDisonnecting);


            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await Task.Delay(-1);
        }

        private async Task OnClientConnected()
        {
            try
            {
                await SendMessageOnGeneralChannels("I'm back online! :nomparty:");
            }
            catch (Exception)
            {
            }
        }
        private void OnClientDisonnecting(object sender, EventArgs e)
        {
            try
            {
                SendMessageOnGeneralChannels(":zzz: I'm going to sleep :zzz:").RunSynchronously();
            }
            catch (Exception)
            {
                //apparently the client closed already!
            }
        }
        
        private async Task SendMessageOnGeneralChannels(string message)
        {
            var generalChannels = _client.GroupChannels.Where(x => x.Name.ToLower().Contains("general"));

            foreach(var channel in generalChannels)
            {
                await channel.SendMessageAsync(message);
            }
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
