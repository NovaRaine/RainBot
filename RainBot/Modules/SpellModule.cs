using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using DiscordHex.Domain;
using RainBot.Domain;
using RainBot.Services;

namespace RainBot.Modules
{
    public class SpellModule : ModuleBase<SocketCommandContext>
    {
        public SpellService SpellService { get; set; }
        public FfxivSpellService FfxivSpellService { get; set; }
        public DiscordSocketClient Discord { get; set; }

        [Command("hex")]
        [Alias("curse")]
        [Summary("Cast a hex on your nemesis, or just on whoever.")]
        [Remarks("hex | hex @user")]
        public async Task CastHex(params string[] message)
        {
            var embedded = SpellService.CastSpell(Context, Discord.CurrentUser.Id, SpellTypeEnum.Hex);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("buff")]
        [Summary("Cast a buff on your target")]
        [Remarks("buff | buff @user")]
        public async Task CastBuff(params string[] message)
        {
            var embedded = SpellService.CastSpell(Context, Discord.CurrentUser.Id, SpellTypeEnum.Buff);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("dd")]
        [Summary("Cast a damage spell on your nemesis, or just on whoever.")]
        [Remarks("dd | dd @user")]
        public async Task CastDirectDamage(params string[] message)
        {
            var embedded = SpellService.CastSpell(Context, Discord.CurrentUser.Id, SpellTypeEnum.DirectDamage);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("esuna")]
        [Summary("Cast Esuna on someone to remove a Negative effect. Possibly..")]
        [Remarks("dd | dd @user")]
        public async Task CastEsuna(params string[] message)
        {
            var embedded = FfxivSpellService.CastEsuna(Context.Message.MentionedUsers, Context.Message.MentionedRoles, Context.Message.Author.Username);
            await ReplyAsync("", false, embedded.Build());
        }
    }
}