using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appccelerate.StateMachine;
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

        public OwnerService()
        {
            SetupStateMachine();
            _praise = new List<string>
            {
                "Thanks mommy! :heart:",
                "good mommy!",
                ":smiley:"
            };

            _scold = new List<string>
            {
                "Sorry mommy :cry:",
                ":cry:",
                "Sorry, I'll try to behave!"
            };
        }

        private void SetupStateMachine()
        {
            _fsm = new PassiveStateMachine<States, Events>();
            _fsm.In(States.Start).On(Events.good).Goto(States.Positive);
            _fsm.In(States.Start).On(Events.bad).Goto(States.Negative);
            _fsm.In(States.Start).On(Events.@do).Goto(States.Question);

            _fsm.In(States.Positive).On(Events.morning).Goto(States.Start).Execute(() => GreetOwner(EventInput.Morning));
            _fsm.In(States.Positive).On(Events.evening).Goto(States.Start).Execute(() => GreetOwner(EventInput.Evening));
            _fsm.In(States.Positive).On(Events.bot).Goto(States.Start).Execute(() => BotFeedback(EventInput.Positive));
            _fsm.In(States.Negative).On(Events.bot).Goto(States.Start).Execute(() => BotFeedback(EventInput.Negative));

            _fsm.In(States.Question).On(Events.love).Goto(States.Start).Execute(() => Question(EventInput.Love));

            _fsm.In(States.Start).On(Events.pat).Goto(States.Start).Execute(PatUser);

            _fsm.Initialize(States.Start);
            _fsm.Start();
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
                _context.Channel.SendMessageAsync("-gently pats Nova on the head-");
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

        private void GreetOwner(EventInput type)
        {
            switch (type)
            {
                case EventInput.Morning:
                    _context.Channel.SendMessageAsync("good morning mommy :smiley:\nLet's have fun today!");
                    break;
                case EventInput.Evening:
                    _context.Channel.SendMessageAsync("good evening mommy :smiley:\nBedtime soon?");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public async Task HandleOwnerMessages(SocketCommandContext context)
        {
            _context = context;

            var message = context.Message;

            if (message.Content.ToLower().Contains("rainbot"))
            {
                ParseComment(RemoveSpecialCharacters(message.Content));
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
        }

        private static string RemoveSpecialCharacters(string str)
        {
            return str.Replace("?", "").Replace("!", "").Replace(".", "").Replace(",", "");
        }
    }
}