using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Commands;
using RainBot.Core;
using RainBot.Data;
using RainBot.Domain;

namespace RainBot.Services
{
    public class SpellService
    {
        private List<SpellEntity> Hexes { get; set; }
        private List<SpellEntity> Buffs { get; set; }
        private List<SpellEntity> DirectDamage { get; set; }

        public SpellService(SpellsContext context)
        {
            var spellRepository = new SpellRepository(context);
            var spells = spellRepository.GetSpells();

            Hexes = spells.Where(x => x.Type == SpellTypeEnum.Hex).ToList();
            Buffs = spells.Where(x => x.Type == SpellTypeEnum.Buff).ToList();
            DirectDamage = spells.Where(x => x.Type == SpellTypeEnum.DirectDamage).ToList();
        }

        public EmbedBuilder CastSpell(SocketCommandContext context, ulong botId, SpellTypeEnum type)
        {
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

            SpellEntity spell;

            switch (type)
            {
                case SpellTypeEnum.Hex:
                    spell = Hexes.ElementAt(Singleton.I.RandomNumber.Next(0, Hexes.Count));
                    break;

                case SpellTypeEnum.DirectDamage:
                    spell = DirectDamage.ElementAt(Singleton.I.RandomNumber.Next(0, DirectDamage.Count));
                    break;

                case SpellTypeEnum.Buff:
                    spell = Buffs.ElementAt(Singleton.I.RandomNumber.Next(0, Buffs.Count));
                    break;

                default:
                    e.Description = "I seem to have forgotten how to cast spells.. wow :/";
                    return e;
            }

            var targets = context.Message.MentionedUsers.Any() ? string.Join(", ", context.Message.MentionedUsers.Select(x => x.Username)) : string.Empty;

            var hours = Singleton.I.RandomNumber.Next(1, 15);
            var duration = "";
            if (type == SpellTypeEnum.Buff || type == SpellTypeEnum.Hex)
                duration = $"\nIt will last for {hours} hours.";

            if (string.IsNullOrEmpty(targets))
            {
                e.Description = type == SpellTypeEnum.Buff
                    ? $"Nya'll get a buff! {spell.Name} on everyone :3{duration}"
                    : $"I cast {spell.Name} on nya'll! Foolish mortals.{duration}";
                return e;
            }

            e.Description = $"{targets}! I cast {spell.Name} on you!{duration} Nyaaa~";

            return e;
        }
    }
}