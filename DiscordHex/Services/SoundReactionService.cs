using Discord;
using System.Collections.Generic;

namespace DiscordHex.Services
{
    public class SoundReactionService
    {
        private Dictionary<string, string> GaySounds;
        private Dictionary<string, string> TransSounds;

        public SoundReactionService()
        {
            GaySounds = new Dictionary<string, string>
            {
                { "happy","https://i.imgur.com/LWZ43Ah.jpg" },
                { "sad","https://i.imgur.com/yuxwthJ.png" },
                { "angry","https://i.imgur.com/m9HmpUU.jpg" },
                { "devious","https://i.imgur.com/nVojgXA.jpg" },
                { "dismissive","https://i.imgur.com/jtF9otr.jpg" },
                { "confused","https://i.imgur.com/1tqZsa6.jpg" },
                { "irritated","https://i.imgur.com/ynAFapI.jpg" },
                { "lewd","https://i.imgur.com/ytQkyLQ.jpg" },
                { "flirty","https://i.imgur.com/WMCPbFE.jpg" },
                { "nightmarish","https://i.imgur.com/PQKRYud.jpg" },
                { "conflicted","https://i.imgur.com/t5zHmVJ.jpg" },
                { "silence","https://i.imgur.com/HGlWIah.jpg" },
                { "tired","https://i.imgur.com/YKLbyCD.jpg" },
                { "imperial","https://i.imgur.com/ybOEIzM.jpg" },
                { "terrified","https://i.imgur.com/S1VO2rp.png" },
                { "festive","https://i.imgur.com/YtuMuUG.png" },
                { "diabolical","https://i.imgur.com/THJxXB5.jpg" },
                { "straight","https://i.imgur.com/iC5N1YI.jpg" },
                { "superhappy", "https://i.imgur.com/dFNgHao.jpg" },
                { "highspeed","https://i.imgur.com/6dmdPJf.jpg" }
            };

            TransSounds = new Dictionary<string, string>
            {
                { "happy", "https://i.imgur.com/e8rzlWk.jpg" },
                { "angry", "https://i.imgur.com/Wve21VD.jpg" },
                { "uneasy", "https://i.imgur.com/qGcEiyd.jpg" },
                { "surprised", "https://i.imgur.com/vt6wvXs.jpg" },
                { "lewd", "https://i.imgur.com/Sl24o29.gif" },
                { "festive", "https://i.imgur.com/vJi1uIe.gif" },
                { "smug", "https://i.imgur.com/CZAWHOs.jpg" },
                { "perfect", "https://i.imgur.com/b2m8iJ0.jpg" },
                { "terrified", "https://i.imgur.com/ajGUAqi.jpg" },
                { "dissapointed", "https://i.imgur.com/ZNu0gvN.jpg" },
                { "banhammer", "https://i.imgur.com/0yoFZfk.jpg" }
            };
        }

        public EmbedBuilder GetTransSounds(string type)
        {
            TransSounds.TryGetValue(type.ToLower(), out var url);
            return BuildEmbedded(url, type);
        }

        public EmbedBuilder GetGaySounds(string type)
        {
            GaySounds.TryGetValue(type.ToLower(), out var url);
            return BuildEmbedded(url, type);
        }

        private EmbedBuilder BuildEmbedded(string url, string type)
        {
            var e = new EmbedBuilder();
            if (!string.IsNullOrEmpty(url))
                e.ImageUrl = url;
            else
                e.Description = $"What do you mean '{type}'.. {type} is not a thing.";

            return e;
        }
    }
}
