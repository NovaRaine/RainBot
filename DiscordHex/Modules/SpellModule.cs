using Discord.Commands;
using Discord.WebSocket;
using DiscordHex.Domain;
using DiscordHex.Services;
using System.Threading.Tasks;

namespace DiscordHex.Modules
{
    public class SpellModule : ModuleBase<SocketCommandContext>
    {
        public SpellService SpellService { get; set; }
        public FfxivSpellService FfxivSpellService { get; set; }
        public DiscordSocketClient Discord { get; set; }


        [Command("hex")]
        [Alias("curse")]
        [Summary("Cast a hex on your nemesis, or just on whoever.")]
        public async Task CastHex(params string[] message)
        {
            var embedded = SpellService.CastSpell(Context, Discord.CurrentUser.Id, SpellType.Hex);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("buff")]
        [Summary("Cast a buff on your target")]
        public async Task CastBuff(params string[] message)
        {
            var embedded = SpellService.CastSpell(Context, Discord.CurrentUser.Id, SpellType.Buff);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("dd")]
        [Summary("Cast a damage spell on your nemesis, or just on whoever.")]
        public async Task CastDirectDamage(params string[] message)
        {
            var embedded = SpellService.CastSpell(Context, Discord.CurrentUser.Id, SpellType.DirectDamage);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("esuna")]
        [Summary("Cast Esuna on someone to remove a negative effect. Possibly..")]
        public async Task CastEsuna(params string[] message)
        {
            var embedded = FfxivSpellService.CastEsuna(Context.Message.MentionedUsers, Context.Message.MentionedRoles, Context.Message.Author.Username);
            await ReplyAsync("", false, embedded.Build());
        }
    }
}
