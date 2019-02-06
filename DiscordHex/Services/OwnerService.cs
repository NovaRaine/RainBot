using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appccelerate.StateMachine;
using Discord;
using Discord.Commands;
using DiscordHex.Core;
using DiscordHex.Domain;

namespace DiscordHex.Services
{
    public class OwnerService
    {
        private readonly List<string> _praise;
        private readonly List<string> _scold;
        private PassiveStateMachine<States, Events> _fsm;
        private SocketCommandContext _context { get; set; }

        public HelpService HelpService { get; set; }
        public RainFactService FactService { get; set; }

        public OwnerService(HelpService helpService, RainFactService rainFactService)
        {
            HelpService = helpService;
            FactService = rainFactService;

            SetupStateMachine();

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

        private void SetupStateMachine()
        {
            _fsm = new PassiveStateMachine<States, Events>();
            _fsm.In(States.Start).On(Events.good).Goto(States.Positive);
            _fsm.In(States.Start).On(Events.bad).Goto(States.Negative);
            _fsm.In(States.Start).On(Events.who).Goto(States.Who);
            _fsm.In(States.Start).On(Events.what).Goto(States.What);
            _fsm.In(States.Start).On(Events.tell).Goto(States.Info);

            _fsm.In(States.Start).On(Events.pat).Goto(States.Start).Execute(() => PatUser());

            _fsm.In(States.Info).On(Events.yourself).Goto(States.Start).Execute(() => RainFact());
            _fsm.In(States.Info).On(Events.you).Goto(States.Start).Execute(() => RainFact());
            _fsm.In(States.Info).On(Events.nova).Goto(States.Start).Execute(() => AboutAuthor());

            _fsm.In(States.Who).On(Events.you).Goto(States.Start).Execute(() => AboutRainbot());
            _fsm.In(States.Who).On(Events.nova).Goto(States.Start).Execute(() => AboutAuthor());

            _fsm.In(States.What).On(Events.you).Goto(States.Abilities);
            _fsm.In(States.Abilities).On(Events.@do).Goto(States.Start).Execute(() => ShowHelp());

            _fsm.In(States.Positive).On(Events.morning).Goto(States.Start).Execute(() => GreetUser(EventInput.Morning));
            _fsm.In(States.Positive).On(Events.evening).Goto(States.Start).Execute(() => GreetUser(EventInput.Evening));
            _fsm.In(States.Positive).On(Events.night).Goto(States.Start).Execute(() => GreetUser(EventInput.Night));

            _fsm.In(States.Positive).On(Events.bot).Goto(States.Start).Execute(() => BotFeedback(EventInput.Positive));
            _fsm.In(States.Negative).On(Events.bot).Goto(States.Start).Execute(() => BotFeedback(EventInput.Negative));

            _fsm.Initialize(States.Start);
            _fsm.Start();
        }

        private void AboutAuthor()
        {
            var text = BotConfig.GetValue("AboutNova");
            _context.Channel.SendMessageAsync(text);

        }

        private void RainFact()
        {
            _context.Channel.SendMessageAsync(FactService.GetFact());
        }

        private void ShowHelp()
        {
            _context.Channel.SendMessageAsync("I sent you a DM :smiley:");
            _context.Channel.SendMessageAsync("", false, HelpService.GetHelp());
        }

        private void AboutRainbot()
        {
            var e = new EmbedBuilder()
                .WithColor(Color.Purple)
                .WithDescription(BotConfig.GetValue("About"))
                .WithTitle("About me, RainBot. A bot")
                .Build();


            _context.Channel.SendMessageAsync("", false, e);
        }

        private void Question(EventInput type)
        {
            switch (type)
            {
                case EventInput.Love:
                    _context.Channel.SendMessageAsync("Yup, without a doubt!");
                    break;
            }
        }

        private void PatUser()
        {
            _context.Channel.SendMessageAsync("Of course! With pleasure :smiley:");

            if (_context.Message.MentionedUsers.Any())
                _context.Channel.SendMessageAsync($"-gently pats {_context.Message.MentionedUsers.First().Username} on the head-");
            else
                _context.Channel.SendMessageAsync($"-gently pats you on the head-");
        }

        private void BotFeedback(EventInput type)
        {
            switch (type)
            {
                case EventInput.Negative:
                    _context.Channel.SendMessageAsync(_scold.ElementAt(Singleton.I.RandomNumber.Next(0, _scold.Count)));
                    break;
                case EventInput.Positive:
                    _context.Channel.SendMessageAsync(_praise.ElementAt(Singleton.I.RandomNumber.Next(0, _praise.Count)));
                    break;
            }
        }

        private void GreetUser(EventInput type)
        {
            switch (type)
            {
                case EventInput.Morning:
                    _context.Channel.SendMessageAsync($"Good morning {GetUser()} :smiley:\nLet's have fun today!");
                    break;
                case EventInput.Evening:
                    _context.Channel.SendMessageAsync($"Good evening {GetUser()} :smiley:\nBedtime soon?");
                    break;
                case EventInput.Night:
                    _context.Channel.SendMessageAsync($"Good night {GetUser()}\n-good night hugs-");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private string GetUser()
        {
            if (BotConfig.IsDragonMom(_context.Message.Author.Id)) {
                return "Mommy";
            }
            else
            {
                return _context.Message.Author.Username;
            }
                
        }

        public async Task HandleOwnerMessages(SocketCommandContext context)
        {
            _context = context;

            var message = context.Message;

            if (message.Content.ToLower().Contains("rainbot"))
            {
                ParseComment(RemoveSpecialCharacters(message.Content.ToLower()));
            }
        }

        public void ParseComment(string message)
        {
            foreach (var word in message.Split(" "))
            {
                if (Enum.TryParse(word, out Events eventType))
                {
                    _fsm.Fire(eventType);
                }
            }
            if (_fsm.IsRunning)
                _fsm.Stop();
            SetupStateMachine();
        }

        private static string RemoveSpecialCharacters(string str)
        {
            return str.Replace("?", "").Replace("!", "").Replace(".", "").Replace(",", "");
        }
    }
}