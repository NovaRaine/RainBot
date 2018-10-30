using Discord;
using DiscordHex.Core;
using DiscordHex.Data;
using DiscordHex.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Services
{
    public class SoundReactionService
    {
        private List<SoundReactEntity> GaySounds;
        private List<SoundReactEntity> TransSounds;

        public SoundReactionService()
        {
            var repo = new SoundReactRepository();

            var sounds = repo.GetSoundReacts();

            GaySounds = sounds.Where(x => x.Type == SoundReactTypeEnum.GAY).ToList();
            TransSounds = sounds.Where(x => x.Type == SoundReactTypeEnum.TRANS).ToList();
        }

        public EmbedBuilder GetTransSounds(string type)
        {
            var sounds = TransSounds.Where(x => x.Name.IndexOf(type, StringComparison.InvariantCultureIgnoreCase) != -1);
            return BuildEmbedded(sounds, type);
        }

        public EmbedBuilder GetGaySounds(string type)
        {
            var sounds = GaySounds.Where(x => x.Name.IndexOf(type, StringComparison.InvariantCultureIgnoreCase) != -1);
            return BuildEmbedded(sounds, type);
        }

        private EmbedBuilder BuildEmbedded(IEnumerable<SoundReactEntity> sounds, string type)
        {
            var e = new EmbedBuilder();
            if (sounds.Any())
            {
                var sound = sounds.ElementAt(BotSettings.Instance.RandomNumber.Next(0, sounds.Count()));
                e.ImageUrl = sound.Url;
            }
            else
            {
                e.Description = $"Could not find a react of type: '{type}'.";
            }
            return e;
        }
    }
}
