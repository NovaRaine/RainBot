using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordHex.Core;
using DiscordHex.Domain;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Services
{
    public class SpellService
    {
        public EmbedBuilder CastSpell(SocketCommandContext context, ulong botId, SpellType type)
        {
            var e = new EmbedBuilder();

            if (context.Message.MentionedUsers != null && context.Message.MentionedUsers.Count > 0 && context.Message.MentionedUsers.Any(x => x.Id == botId) && type != SpellType.Buff)
            {
                e.Description = "You're targeting me? But.. but why.. what have I done :(";
                return e;
            }

            if ((type == SpellType.DirectDamage || type == SpellType.Hex) && context.Message.MentionedUsers.Any(x => x.Id == 462658205009575946))
            {
                e.Description = $"{context.Message.Author.Username} tried to harm Dragon Mom!";
                e.ImageUrl = "https://media3.giphy.com/media/26Bnc1dYtp3SvCfDi/giphy.gif";
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

            var targets = context.Message.MentionedUsers.Any() ? string.Join(", ", context.Message.MentionedUsers.Select(x => x.Username)) : string.Empty;

            if (string.IsNullOrEmpty(targets))
            {
                if (type == SpellType.Buff)
                    e.Description = $"Nya'll get a buff! {spell.Name} on everyone :3";
                else
                    e.Description = $"I cast {spell.Name} on you all! Foolish mortals.";
                return e;
            }

            e.Description = $"{targets}! I cast {spell.Name} on you!";
            e.ImageUrl = string.IsNullOrEmpty(spell.Img) ? "" : spell.Img;
            return e;
        }
    }
}
