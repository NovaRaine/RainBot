﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RainBot.ChatBot;
using RainBot.Core;
using RainBot.Data;
using RainBot.Services;

namespace RainBot
{
    public class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            SetupLogging();

            Environment.SetEnvironmentVariable("Version", "4.2_L");

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

                .AddSingleton<RandomPictureService>()
                .AddSingleton<SpellService>()
                .AddSingleton<FfxivSpellService>()
                .AddSingleton<SoundReactionService>()
                .AddSingleton<HelpService>()
                .AddSingleton<RainFactService>()

                .AddSingleton<ChatServices>()
                .AddSingleton<ChatHandler>()
                .AddSingleton<MoodService>()

                .AddDbContext<RainBotContext>(opt => opt.UseSqlServer(BotConfig.GetValue("ConnectionString")))

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