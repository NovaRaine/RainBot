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
        private readonly List<SoundReactEntity> _gaySounds;
        private readonly List<SoundReactEntity> _transSounds;
        private readonly List<SoundReactEntity> _naps;

        public SoundReactionService(BotContext dbContext)
        {
            var repo = new SoundReactRepository(dbContext);
            var sounds = repo.GetSoundReacts();

            _gaySounds = sounds.Where(x => x.Type == SoundReactTypeEnum.GAY).ToList();
            _transSounds = sounds.Where(x => x.Type == SoundReactTypeEnum.TRANS).ToList();
            _naps = sounds.Where(x => x.Type == SoundReactTypeEnum.NAP).ToList();
        }

        public EmbedBuilder GetRandomNap()
        {
            return BuildEmbedded(_naps, "nap");
        }

        public EmbedBuilder GetTransSounds(string type)
        {
            if (type.ToLower().Trim() == "list")
                return GetList(SoundReactTypeEnum.TRANS);

            var sounds = _transSounds.Where(x => x.Name.IndexOf(type, StringComparison.InvariantCultureIgnoreCase) != -1);
            return BuildEmbedded(sounds, type);
        }

        public EmbedBuilder GetGaySounds(string type)
        {
            if (type.ToLower().Trim() == "list")
                return GetList(SoundReactTypeEnum.GAY);

            var sounds = _gaySounds.Where(x => x.Name.IndexOf(type, StringComparison.InvariantCultureIgnoreCase) != -1);
            return BuildEmbedded(sounds, type);
        }

        private EmbedBuilder GetList(SoundReactTypeEnum type)
        {
            var emb = new EmbedBuilder {Title = "List"};

            switch (type)
            {
                case SoundReactTypeEnum.GAY:
                    emb.Description = string.Join(", ", _gaySounds.Select(x => x.Name).ToList());
                    break;
                case SoundReactTypeEnum.TRANS:
                    emb.Description = string.Join(", ", _transSounds.Select(x => x.Name).ToList());
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
            var sound = sounds.ElementAt(BotSettings.Instance.RandomNumber.Next(0, sounds.Count()));

            if (sound != null)
            {
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
