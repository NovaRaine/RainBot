using Discord;
using Discord.Commands;
using DiscordHex.Core;
using DiscordHex.Data;
using DiscordHex.Domain;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Services
{
    public class SpellService
    {
        private List<SpellEntity> Hexes { get; set; }
        private List<SpellEntity> Buffs { get; set; }
        private List<SpellEntity> DirectDamage { get; set; }
        private ProfileService _profileService;

        public SpellService()
        {
            _profileService = new ProfileService();
            var spellRepository = new SpellRepository();
            var spells = spellRepository.GetSpells();

            Hexes = spells.Where(x => x.Type == SpellTypeEnum.Hex).ToList();
            Buffs = spells.Where(x => x.Type == SpellTypeEnum.Buff).ToList();
            DirectDamage = spells.Where(x => x.Type == SpellTypeEnum.DirectDamage).ToList();
        }

        public EmbedBuilder CastSpell(SocketCommandContext context, ulong botId, SpellTypeEnum type)
        {
            _profileService.IncreaseCount(context.Message.Author.Id, type, true);
            if (context.Message.MentionedUsers.Count > 0)
                _profileService.IncreaseCount(context.Message.MentionedUsers.First().Id, type, false);

            var e = new EmbedBuilder();

            if (context.Message.MentionedUsers != null && context.Message.MentionedUsers.Count > 0 && context.Message.MentionedUsers.Any(x => x.Id == botId) && type != SpellTypeEnum.Buff)
            {
                e.Description = "You're targeting me? But.. but why.. what have I done :(";
                return e;
            }

            if (context.Message.MentionedRoles.Count > 0) // no mentioning roles 
            {
                e.Description = "I'm not targeting a whole group, and you should know better.. Shame on you :(";
                return e;
            }

            SpellEntity spell = null;
            
            switch (type)
            {
                case SpellTypeEnum.Hex:
                    spell = Hexes.ElementAt(BotSettings.Instance.RandomNumber.Next(0, Hexes.Count));
                    break;
                case SpellTypeEnum.DirectDamage:
                    spell =DirectDamage.ElementAt(BotSettings.Instance.RandomNumber.Next(0, DirectDamage.Count));
                    break;
                case SpellTypeEnum.Buff:
                    spell = Buffs.ElementAt(BotSettings.Instance.RandomNumber.Next(0, Buffs.Count));
                    break;
                default:
                    break;
            }
            
            if (spell == null)
            {
                e.Description = "I seem to have forgotten how to cast spells.. wow :/";
                return e;
            }

            var targets = context.Message.MentionedUsers.Any() ? string.Join(", ", context.Message.MentionedUsers.Select(x => x.Username)) : string.Empty;

            var hours = BotSettings.Instance.RandomNumber.Next(1, 15);
            var duration = "";
            if (type == SpellTypeEnum.Buff || type == SpellTypeEnum.Hex)
                duration = $"\nIt will last for {hours} hours.";

            if (string.IsNullOrEmpty(targets))
            {
                if (type == SpellTypeEnum.Buff)
                    e.Description = $"Nya'll get a buff! {spell.Name} on everyone :3{duration}";
                else
                    e.Description = $"I cast {spell.Name} on you all! Foolish mortals.{duration}";
                return e;
            }

            e.Description = $"{targets}! I cast {spell.Name} on you!{duration}";
            _profileService.AddSpellEffect(context.Message.MentionedUsers, spell.Name, hours);

            e.ImageUrl = string.IsNullOrEmpty(spell.Img) ? "" : spell.Img;
            return e;
        }
    }
}
