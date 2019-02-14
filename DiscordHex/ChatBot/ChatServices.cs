using Discord;
using Discord.Commands;
using DiscordHex.Core;
using DiscordHex.Domain;
using DiscordHex.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordHex.ChatBot
{
    public class ChatServices
    {
        public HelpService HelpService { get; set; }
        public RainFactService FactService { get; set; }
        public SocketCommandContext Context { get; set; }

        private readonly List<string> _praise;
        private readonly List<string> _scold;

        public ChatServices(HelpService helpService, RainFactService rainFactService)
        {
            HelpService = helpService;
            FactService = rainFactService;

            _praise = new List<string>
            {
                "Thanks! :heart:",
                "good human!",
                ":smiley:"
            };

            _scold = new List<string>
            {
                "Sorry :cry:",
                ":cry:",
                "Sorry, I'll try to behave!"
            };
        }

        private string GetUser()
        {
            if (BotConfig.IsDragonMom(Context.Message.Author.Id))
            {
                return "Mommy";
            }
            else
            {
                return Context.Message.Author.Username;
            }

        }

        public void GreetUser(EventInput type)
        {
            switch (type)
            {
                case EventInput.Morning:
                    Context.Channel.SendMessageAsync($"Good morning {GetUser()} :smiley:\nLet's have fun today!");
                    break;
                case EventInput.Evening:
                    Context.Channel.SendMessageAsync($"Good evening {GetUser()} :smiley:\nBedtime soon?");
                    break;
                case EventInput.Night:
                    Context.Channel.SendMessageAsync($"Good night {GetUser()}\n-good night hugs-");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void BotFeedback(EventInput type)
        {
            switch (type)
            {
                case EventInput.Negative:
                    Context.Channel.SendMessageAsync(_scold.ElementAt(Singleton.I.RandomNumber.Next(0, _scold.Count)));
                    break;
                case EventInput.Positive:
                    Context.Channel.SendMessageAsync(_praise.ElementAt(Singleton.I.RandomNumber.Next(0, _praise.Count)));
                    break;
            }
        }

        public void PatUser()
        {
            Context.Channel.SendMessageAsync("Of course! With pleasure :smiley:");

            if (Context.Message.MentionedUsers.Any())
                Context.Channel.SendMessageAsync($"-gently pats {Context.Message.MentionedUsers.First().Username} on the head-");
            else
                Context.Channel.SendMessageAsync($"-gently pats you on the head-");
        }

        public void AboutRainbot()
        {
            var e = new EmbedBuilder()
                .WithColor(Color.Purple)
                .WithDescription(BotConfig.GetValue("About"))
                .WithTitle("About me, RainBot. A bot")
                .Build();


            Context.Channel.SendMessageAsync("", false, e);
        }

        public void ShowHelp()
        {
            Context.Channel.SendMessageAsync("I sent you a DM :smiley:");
            Context.Message.Author.SendMessageAsync("", false, HelpService.GetHelp());
        }

        public void RainFact()
        {
            Context.Channel.SendMessageAsync(FactService.GetFact());
        }

        public void AboutAuthor()
        {
            var text = BotConfig.GetValue("AboutNova");
            Context.Channel.SendMessageAsync(text);
        }
    }
}
