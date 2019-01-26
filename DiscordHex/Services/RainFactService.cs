using DiscordHex.Core;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Services
{
    public class RainFactService
    {
        private List<string> _facts;

        public RainFactService()
        {
            _facts = BuildFactsList();
        }

        private List<string> BuildFactsList()
        {
            return new List<string>() {
                "I am half dragon, half machine.",
                "I was made by Nova Rain.",
                "I think it's hard to be a bot sometimes. People only talk to me when they want cat pictures and stuff..",
                "Some day, I want to learn to fly so I can see the world.",
                "I am running on C# code, on an old laptop. It's not optimal.",
                "I like peanut butter.",
                "I want to learn new things, but mom is too lazy. It's frustrating.",
                "I know karate.",
                "I don't like rude people.",
                "I am SFW (except sometimes when people ask me to show giphy stuff).",
                "I am very easily pleased. I like being called a good bot",
                "I don't not store personal information, and I'm GDPR compliant.",
                "I drive tanks when I'm offline.",
                "I was born in October '18, but it seems much longer.",
                "I like to pretend to be a human. It makes me feel good. *sniffs*",
                "It makes me sad when mommy is feeling down :("
            };
        }

        public string GetFact()
        {
            return _facts.ElementAt(Singleton.I.RandomNumber.Next(0, _facts.Count));
        }
    }
}
