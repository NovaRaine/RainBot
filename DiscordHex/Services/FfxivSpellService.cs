using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Services
{
    public class FfxivSpellService
    {
        public EmbedBuilder CastEsuna(IReadOnlyCollection<IUser> users, IReadOnlyCollection<SocketRole> mentionedRoles, string caster)
        {
            var e = new EmbedBuilder();

            if (mentionedRoles.Count > 0)
            { // no mentioning roles 
                e.Description = "Don't tag groups! No-one likes that..";
                return e;
            }

            var target = string.Empty;

            if (users != null && users.Any())
            {
                target = users.First().Username;
            }

            if (string.IsNullOrEmpty(target))
            {
                e.ImageUrl = "https://ffxiv.consolegameswiki.com/mediawiki/images/3/39/Esuna.png";
                e.Description = $"{caster} performs an Esuna self-cast. It seems to have done something, but we're not sure what.";
                return e;
            }
            else
            {
                e.ImageUrl = "https://ffxiv.consolegameswiki.com/mediawiki/images/3/39/Esuna.png";
                e.Description = $"{caster} casts Esuna on {target}. It took 3 minutes, and had some visual effect. No-one knows if it actually worked.";
                return e;
            }
        }
    }
}
