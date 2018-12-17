using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordHex.Core
{
    internal class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            CheckSpecialCases(message);

            var prefix = BotConfig.GetValue("Prefix");
            if (string.IsNullOrEmpty(prefix)) return;

            var argPos = prefix.Length;
            
            if (!message.HasStringPrefix(prefix, ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);
            var result = await _commands.ExecuteAsync(context, argPos, _services);

            if (result.Error.HasValue && result.Error.Value != CommandError.UnknownCommand)
            {
                string resMsg;
                switch (result.Error)
                {
                    case CommandError.Exception:
                        Log.Error($"Command error: Reason: {result.ErrorReason} - message: {message} - Author: {message.Author}");
                        resMsg = "Oh snap! I just had some internal epic fail.. Sorry :/";
                        break;
                    case CommandError.Unsuccessful:
                        Log.Warning($"Command error: Reason: {result.ErrorReason} - message: {message} - Author: {message.Author}");
                        resMsg = "Oops, I messed that up.";
                        break;
                    case CommandError.BadArgCount:
                        resMsg = "You messed up the parameters or the order of them :(";
                        break;
                    case CommandError.UnmetPrecondition:
                        resMsg = "Denied! Nyaaa~";
                        break;
                    default:
                        Log.Error($"Command error: Reason: {result.ErrorReason} - message: {message} - Author: {message.Author}");
                        resMsg = "I.. uhm.. What do you want really?";
                        break;
                }
                await context.Channel.SendMessageAsync(resMsg);
            }
        }

        private void CheckSpecialCases(SocketUserMessage message)
        {
            if (message.Author.Id == 462658205009575946)
            {
                var context = new SocketCommandContext(_discord, message);
                if (message.Content.ToLower() == "bad bot")
                    context.Channel.SendMessageAsync("Sorry mommy :(");
                else if (message.Content.ToLower() == "good bot")
                    context.Channel.SendMessageAsync("Thanks mommy! <3");
            }
        }
    }
}
