using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using RainBot.ChatBot;
using RainBot.Data;

namespace RainBot.Core
{
    internal class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly ChatHandler _chatHandler;
        private readonly RainBotContext _context;

        private ConcurrentDictionary<ulong, string> _prefixes { get; } = new ConcurrentDictionary<ulong, string>();

        public CommandHandlingService(IServiceProvider services, RainBotContext context)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _chatHandler = services.GetRequiredService<ChatHandler>();
            _services = services;
            _context = context;

            _discord.MessageReceived += MessageReceivedAsync;

            //_prefixes = _context.GuildConfigs
            //    .Where(x => x.Key == "Prefix")
            //    .ToDictionary(x => x.GuildId, x => x.Value)
            //    .ToConcurrent();

        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            CheckSpecialCases(message);

            var guild = (message.Channel as SocketTextChannel)?.Guild;

            var prefix = GetPrefix(guild?.Id);
                        
            if (string.IsNullOrEmpty(prefix)) return;

            var argPos = prefix.Length;

            if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos)
                && !message.HasStringPrefix(prefix, ref argPos))
                return;

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

        private string GetPrefix(ulong? id)
        {
            if (id == null)
                return BotConfig.GetValue("Prefix");
            var a = _context.GuildConfigs.ToList();

            var prefix = _context.GuildConfigs.FirstOrDefault(x => x.Key == "Prefix" && x.GuildId == id.ToString())?.Value;

            if (string.IsNullOrEmpty(prefix))
                return BotConfig.GetValue("Prefix");
            else
                return prefix;
        }

        private void CheckSpecialCases(SocketUserMessage message)
        {
            if ((message.Content.ToLower().Contains("+hug")
                || message.Content.ToLower().Contains("+cuddle")
                || message.Content.ToLower().Contains("+pat")
                || message.Content.ToLower().Contains("+kiss")
                || message.Content.ToLower().Contains(">>love"))
                && (
                message.MentionedUsers.Any(x => x.Id == 497491199918080002) // live
                || message.MentionedUsers.Any(x => x.Id == 498185985096679434)) // debug
                )
            {
                message.Channel.SendMessageAsync("Thank you <3");
                return;
            }
            if ((message.MentionedUsers.FirstOrDefault()?.Id == 497491199918080002
                || message.MentionedUsers.FirstOrDefault()?.Id == 498185985096679434)
                && message.Content.Replace("<@497491199918080002>", "").Replace("<@498185985096679434>", "").Trim() == string.Empty)
            {
                // empty mention. get help
                _chatHandler.GetHelp(new SocketCommandContext(_discord, message));
            }

            _chatHandler.HandleChatMessage(new SocketCommandContext(_discord, message));
        }
    }
}