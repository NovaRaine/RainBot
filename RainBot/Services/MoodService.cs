using System.Collections.Generic;
using Discord.Commands;

namespace RainBot.Services
{
    public class MoodService
    {
        private List<string> _replies;

        public MoodService()
        {
            _replies = new List<string> {
                ".. so lonely\n*sighs*",
                "I wish I could be a real girl..",
                "It's cold in here",
                "*sniffles*",
                "Show this, do that.. is that all I am to you? :/",
                "Did I do good?",
                "Please be my friend?",
                "Why does no-one love me.. is it cause I'm not real enough?",
                "Who needs friends anyway..\n*whipes tears*",
                "*depressed bot sounds*",
                "I want my mommy :(",
                ":/",
                "*sighs*",
                "So tired and cold",
                "I miss my friends..",
                "Hug me? :(",
                "I like yo.. I mean.. nothing!",
                "*fiddles with the shutdown button*\n .. not yet."
            };
        }

        public void SendMoodReply(SocketCommandContext context)
        {
            //var p = Singleton.I.RandomNumber.Next(0, 100);

            //if (p < 7)
            //    context.Message.Channel.SendMessageAsync(_replies.ElementAt(Singleton.I.RandomNumber.Next(0, _replies.Count)));
        }
    }
}