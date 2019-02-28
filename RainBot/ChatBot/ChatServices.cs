using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Commands;
using RainBot.Core;
using RainBot.Domain;
using RainBot.Services;

namespace RainBot.ChatBot
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

        internal void Execute(Template template)
        {
            switch (template.Execute)
            {
                case TemplateExecutionEnum.GREETING:
                    GreetUser(template.Response);
                    break;

                case TemplateExecutionEnum.ABOUTME:
                    AboutRainbot();
                    break;

                case TemplateExecutionEnum.ABOUTNOVA:
                    AboutAuthor();
                    break;

                case TemplateExecutionEnum.BOTFACT:
                    RainFact();
                    break;

                case TemplateExecutionEnum.HELP:
                    ShowHelp();
                    break;

                case TemplateExecutionEnum.PAT:
                    PatUser();
                    break;

                case TemplateExecutionEnum.BAD:
                    BotFeedback(EventInput.Negative);
                    break;

                case TemplateExecutionEnum.GOOD:
                    BotFeedback(EventInput.Positive);
                    break;

                case TemplateExecutionEnum.SHOW:
                    break; // needs wildcard impl

                case TemplateExecutionEnum.LOVEME:
                    LoveTheBot();
                    break;

                case TemplateExecutionEnum.LOVEOTHER:
                    break;

                case TemplateExecutionEnum.BOTLIKES:
                    DoBotLike();
                    break;

                default:
                    //ignore, no response
                    break;
            }
        }

        private void LoveTheBot()
        {
            if (Context.Message.Author.Id == 462658205009575946)
                Context.Channel.SendMessageAsync("Love you too mom :smiley:");
            else if (Context.Message.Author.Id == 462658205009575946)
                Context.Channel.SendMessageAsync(":heart:");
            else
                Context.Channel.SendMessageAsync("Oh.. awkward.. :flushed:");
        }

        private void DoBotLike()
        {
            if (Context.Message.Author.Id == 462658205009575946)
                Context.Channel.SendMessageAsync("Yes! Best mommy :)");
            else if (Context.Message.Author.Id == 462658205009575946)
                Context.Channel.SendMessageAsync("You'll always be on my 'AcceptedHumanEntities' list Mio :heart:");
            else
                Context.Channel.SendMessageAsync("Uhm.. well, it's complicated really..");
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

        public void GreetUser(string response)
        {
            var resp = string.Format(response, GetUser());

            Context.Channel.SendMessageAsync(resp);
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