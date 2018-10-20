using Discord;
using Discord.WebSocket;
using DiscordHex.Core;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Services
{
    public class SpellService
    {
        public EmbedBuilder CastSpell(IReadOnlyCollection<IUser> users, IReadOnlyCollection<SocketRole> mentionedRoles, ulong botId, SpellType type)
        {
            var e = new EmbedBuilder();

            if (users != null && users.Count > 0 && users.Any(x => x.Id == botId))
            {
                e.Description = "You're targeting me? But.. but why.. what have I done :(";
                return e;
            }

            if ((type == SpellType.DirectDamage || type == SpellType.Hex) && users.Any(x => x.Id == 462658205009575946))
            {
                e.Description = "Hah! You think you can harm the Dragon Mom? A curse on you and your family. May your crops wither and your wells dry out!";
                return e;
            }

            if (mentionedRoles.Count > 0) // no mentioning roles 
            {
                e.Description = "I'm not targeting a whole group, and you should know better.. Shame on you :(";
                return e;
            }

            SpellEntity spell = null;
            
            switch (type)
            {
                case SpellType.Hex:
                    spell = BotSettings.Instance.Hexes.ElementAt(BotSettings.Instance.RandomNumber.Next(0, BotSettings.Instance.Hexes.Count));
                    break;
                case SpellType.DirectDamage:
                    spell = BotSettings.Instance.DirectDamage.ElementAt(BotSettings.Instance.RandomNumber.Next(0, BotSettings.Instance.DirectDamage.Count));
                    break;
                case SpellType.Buff:
                    spell = BotSettings.Instance.Buffs.ElementAt(BotSettings.Instance.RandomNumber.Next(0, BotSettings.Instance.Buffs.Count));
                    break;
                default:
                    break;
            }
            
            if (spell == null)
            {
                e.Description = "I seem to have forgotten how to cast spells.. wow :/";
                return e;
            }

            var targets = users.Any() ? string.Join(", ", users.Select(x => x.Username)) : string.Empty;

            if (string.IsNullOrEmpty(targets))
            {
                if (type == SpellType.Buff)
                    e.Description = $"Nya'll get a buff! {spell.Name} on everyone :3";
                else
                    e.Description = $"I cast {spell.Name} on you all! Foolish mortals.";
                return e;
            }

            e.Description = $"{targets}! I cast {spell.Name} on you!";
            e.ImageUrl = string.IsNullOrEmpty(spell.ImageUrl) ? "https://cdn.awwni.me/nltc.jpg" : spell.ImageUrl;
            return e;
        }
    }

    public class SpellEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public SpellType Type { get; set; }
    }

    public enum SpellType
    {
        Hex = 1,
        Buff = 2,
        DirectDamage = 3
    }
}
