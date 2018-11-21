using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using DiscordHex.Core;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using System.Net.Http;
using DiscordHex.Services;
using log4net.Config;
using log4net;
using System.Reflection;
using System.IO;

namespace DiscordHex
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private DiscordSocketClient _client;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            SetupLogging();

            Environment.SetEnvironmentVariable("Version", "3.11 For Workstations");

            var services = ConfigureServices();

            _client = services.GetRequiredService<DiscordSocketClient>();

            _client.Log += Log;
            services.GetRequiredService<CommandService>().Log += Log;

#if DEBUG
            await _client.LoginAsync(TokenType.Bot, BotConfig.GetValue("DiscordTokenDebug"));
#endif
#if !DEBUG
            await _client.LoginAsync(TokenType.Bot, ConfigLoader.GetValue("DiscordToken"));
#endif

            await _client.StartAsync();
            await _client.SetGameAsync($"{BotConfig.GetValue("Prefix")}help");
            
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await Task.Delay(-1);
        }

        private void SetupLogging()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
#if DEBUG
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.debug.config"));
#endif
#if !DEBUG
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
#endif
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
                .AddSingleton<ProfileService>()

                .BuildServiceProvider();
        }

        private Task Log(LogMessage msg)
        {
            log.Info(msg.Message);
            return Task.CompletedTask;
        }
    }
}
