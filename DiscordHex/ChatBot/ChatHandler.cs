using Appccelerate.StateMachine;
using Discord.Commands;
using DiscordHex.Domain;
using System;

namespace DiscordHex.ChatBot
{
    public class ChatHandler
    {
        private ChatServices _chatServices { get; set; }
        private PassiveStateMachine<States, Events> _fsm;

        public ChatHandler(ChatServices chatServices)
        {
            _chatServices = chatServices;
            
            SetupStateMachine();
        }

        #region Parsing comments
        public void HandleChatMessage(SocketCommandContext context)
        {
            _chatServices.Context = context;

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

        #endregion

        #region State setup
        private void SetupStateMachine()
        {
            _fsm = new PassiveStateMachine<States, Events>();

            SetupPosNeg();
            SetupInfo();
           
            _fsm.In(States.Start).On(Events.pat).Goto(States.Start).Execute(() => _chatServices.PatUser());

            _fsm.Initialize(States.Start);
            _fsm.Start();
        }

        private void SetupPosNeg()
        {
            _fsm.In(States.Start).On(Events.good).Goto(States.Positive);
            _fsm.In(States.Start).On(Events.bad).Goto(States.Negative);

            _fsm.In(States.Positive).On(Events.morning).Goto(States.Start).Execute(() => _chatServices.GreetUser(EventInput.Morning));
            _fsm.In(States.Positive).On(Events.evening).Goto(States.Start).Execute(() => _chatServices.GreetUser(EventInput.Evening));
            _fsm.In(States.Positive).On(Events.night).Goto(States.Start).Execute(() => _chatServices.GreetUser(EventInput.Night));
            _fsm.In(States.Positive).On(Events.bot).Goto(States.Start).Execute(() => _chatServices.BotFeedback(EventInput.Positive));

            _fsm.In(States.Negative).On(Events.bot).Goto(States.Start).Execute(() => _chatServices.BotFeedback(EventInput.Negative));
        }

        private void SetupInfo()
        {
            _fsm.In(States.Start).On(Events.who).Goto(States.Who);
            _fsm.In(States.Start).On(Events.what).Goto(States.What);
            _fsm.In(States.Start).On(Events.tell).Goto(States.Info);

            _fsm.In(States.Info).On(Events.yourself).Goto(States.Start).Execute(() => _chatServices.RainFact());
            _fsm.In(States.Info).On(Events.you).Goto(States.Start).Execute(() => _chatServices.RainFact());
            _fsm.In(States.Info).On(Events.nova).Goto(States.Start).Execute(() => _chatServices.AboutAuthor());

            _fsm.In(States.Who).On(Events.you).Goto(States.Start).Execute(() => _chatServices.AboutRainbot());
            _fsm.In(States.Who).On(Events.nova).Goto(States.Start).Execute(() => _chatServices.AboutAuthor());

            _fsm.In(States.What).On(Events.you).Goto(States.Abilities);
            _fsm.In(States.Abilities).On(Events.@do).Goto(States.Start).Execute(() => _chatServices.ShowHelp());
        }

        #endregion
    }
}
