﻿using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using DiscordHex.Core;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using System.Net.Http;
using DiscordHex.Services;
using DiscordHex.Data;
using DiscordHex.ChatBot;

namespace DiscordHex
{
    public class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            SetupLogging();

            Environment.SetEnvironmentVariable("Version", "4.0_L");

            var services = ConfigureServices();

            _client = services.GetRequiredService<DiscordSocketClient>();

            _client.Log += LogMessage;
            services.GetRequiredService<CommandService>().Log += LogMessage;

            await _client.LoginAsync(TokenType.Bot, BotConfig.GetValue("DiscordToken"));
            await _client.StartAsync();
            await _client.SetGameAsync($"{BotConfig.GetValue("Prefix")}help");
            
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await Task.Delay(-1);
        }

        private void SetupLogging()
        {
            Log.Logger = LoggingProvider.GetLogger();
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()

                .AddSingleton<CommandService>()
                .AddSingleton<CommonCommands>()
                .AddSingleton<BotContext>()
                
                .AddSingleton<RandomPictureService>()
                .AddSingleton<SpellService>()
                .AddSingleton<FfxivSpellService>()
                .AddSingleton<SoundReactionService>()
                .AddSingleton<GameSession>()
                .AddSingleton<ProfileService>()
                .AddSingleton<HelpService>()
                .AddSingleton<RainFactService>()

                .AddSingleton<ChatServices>()
                .AddSingleton<ChatHandler>()
                .AddSingleton<MoodService>()

                .BuildServiceProvider();
        }

        private Task LogMessage(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    Log.Fatal(msg.ToString());
                    break;
                case LogSeverity.Error:
                    Log.Error(msg.ToString());
                    break;
                case LogSeverity.Warning:
                    Log.Warning(msg.ToString());
                    break;
                case LogSeverity.Info:
                    Log.Information(msg.ToString());
                    break;
                case LogSeverity.Verbose:
                    Log.Verbose(msg.ToString());
                    break;
                case LogSeverity.Debug:
                    Log.Debug(msg.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return Task.CompletedTask;
        }
    }
}
