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
        private List<SoundReactEntity> Naps;

        public SoundReactionService()
        {
            var repo = new SoundReactRepository();
            var sounds = repo.GetSoundReacts();

            GaySounds = sounds.Where(x => x.Type == SoundReactTypeEnum.GAY).ToList();
            TransSounds = sounds.Where(x => x.Type == SoundReactTypeEnum.TRANS).ToList();
            Naps = sounds.Where(x => x.Type == SoundReactTypeEnum.NAP).ToList();
        }

        public EmbedBuilder GetRandomNap()
        {
            return BuildEmbedded(Naps, "nap");
        }

        public EmbedBuilder GetTransSounds(string type)
        {
            if (type.ToLower().Trim() == "list")
                return GetList(SoundReactTypeEnum.TRANS);

            var sounds = TransSounds.Where(x => x.Name.IndexOf(type, StringComparison.InvariantCultureIgnoreCase) != -1);
            return BuildEmbedded(sounds, type);
        }

        public EmbedBuilder GetGaySounds(string type)
        {
            if (type.ToLower().Trim() == "list")
                return GetList(SoundReactTypeEnum.GAY);

            var sounds = GaySounds.Where(x => x.Name.IndexOf(type, StringComparison.InvariantCultureIgnoreCase) != -1);
            return BuildEmbedded(sounds, type);
        }

        private EmbedBuilder GetList(SoundReactTypeEnum type)
        {
            var emb = new EmbedBuilder();
            emb.Title = "List";

            switch (type)
            {
                case SoundReactTypeEnum.GAY:
                    emb.Description = string.Join(", ", GaySounds.Select(x => x.Name).ToList());
                    break;
                case SoundReactTypeEnum.TRANS:
                    emb.Description = string.Join(", ", TransSounds.Select(x => x.Name).ToList());
                    break;
                default:
                    emb.Description = "Epic internal fail!";
                    break;
            }

            return emb;
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
