﻿using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using RainBot.Core;
using RainBot.Data;
using RainBot.Domain;

namespace RainBot.Services
{
    public class SoundReactionService
    {
        private readonly List<SoundReactEntity> _gaySounds;
        private readonly List<SoundReactEntity> _transSounds;
        private readonly List<SoundReactEntity> _naps;
        private readonly List<SoundReactEntity> _miwa;

        public SoundReactionService(RainBotContext context)
        {
            var repo = new SoundReactRepository(context);
            var sounds = repo.GetSoundReacts();

            _gaySounds = sounds.Where(x => x.Type == SoundReactTypeEnum.GAY).ToList();
            _transSounds = sounds.Where(x => x.Type == SoundReactTypeEnum.TRANS).ToList();
            _naps = sounds.Where(x => x.Type == SoundReactTypeEnum.NAP).ToList();
            _miwa = new List<SoundReactEntity>
            {
                new SoundReactEntity { Url = "https://storage.googleapis.com/gsposts/naps/Nap6.png"},
                new SoundReactEntity { Url = "https://storage.googleapis.com/gsposts/naps/nap1.gif"},
                new SoundReactEntity { Url = "https://storage.googleapis.com/gsposts/naps/nap12.jpg"},
                new SoundReactEntity { Url = "https://storage.googleapis.com/gsposts/naps/nap16.jpg"},
                new SoundReactEntity { Url = "https://storage.googleapis.com/gsposts/naps/nap2.gif"},
                new SoundReactEntity { Url = "https://storage.googleapis.com/gsposts/naps/nap5.gif"},
                new SoundReactEntity { Url = "https://storage.googleapis.com/gsposts/naps/nap7.gif"}
            };
        }

        public EmbedBuilder GetNap(ulong id)
        {
            switch (id)
            {
                case 349189242624147456:
                    return BuildEmbedded(_miwa, "nap");

                default:
                    return BuildEmbedded(_naps, "nap");
            }
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
            var emb = new EmbedBuilder { Title = "List" };

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
            SoundReactEntity sound = null;
            if (sounds.Any())
            {
                sound = sounds.ElementAt(Singleton.I.RandomNumber.Next(0, sounds.Count()));
            }

            if (sound != null)
            {
                e.ImageUrl = sound.Url;
            }
            else
            {
                e.Description = $"Could not find a react matching '{type}'.";
            }

            return e;
        }
    }
}