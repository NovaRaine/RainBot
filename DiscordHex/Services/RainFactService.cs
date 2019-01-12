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
                "RainBot is half dragon, half machine.",
                "RainBot is made by Nova Rain.",
                "RainBot thinks it's hard to be a bot sometimes. 'People only talk to me when they want cat pictures and stuff..'",
                "Some day, RainBot want to learn to fly so she can see the world.",
                "RainBot is running on C# code, on an old laptop. It's not optimal.",
                "RainBot likes peanut butter.",
                "RainBot wants to learn new things, but her mom is too lazy. It's frustrating for RainBot",
                "RainBot knows karate.",
                "RainBot don't like rude people.",
                "RainBot is SFW (except sometimes when people ask her to show giphy stuff).",
                "RainBot is very easily pleased. She likes being called a good bot",
                "RainBot does not store personal information, and is GDPR compliant.",
                "RainBot drives tanks when she's offline.",
                "RainBot was born in October 18, but it seems much longer.",
                "RainBot likes to pretend to be a human. It makes her feel good."
            };
        }

        public string GetFact()
        {
            return _facts.ElementAt(Singleton.I.RandomNumber.Next(0, _facts.Count));
        }
    }
}
