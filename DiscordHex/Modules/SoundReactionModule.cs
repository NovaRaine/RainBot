using Discord.Commands;
using DiscordHex.Services;
using System.Threading.Tasks;


namespace DiscordHex.Modules
{
    public class SoundReactionModule : ModuleBase<SocketCommandContext>
    {
        public SoundReactionService SoundReactionService { get; set; }

        public SoundReactionModule(SoundReactionService soundReactionService)
        {
            SoundReactionService = soundReactionService;
        }

        [Command("gs")]
        [Alias("gaysounds")]
        [Summary("Show your gayness in a react image.")]
        [Remarks("gs [type] example: gs sad")]
        public async Task GaySounds(params string [] type)
        {
            if (type.Length == 0)
                return;

            var embedded = SoundReactionService.GetGaySounds(type[0]);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("ts")]
        [Alias("transsounds")]
        [RequireOwner]
        public async Task TransSounds(params string[] type)
        {
            if (type.Length == 0)
                return;

            var embedded = SoundReactionService.GetTransSounds(type[0]);
            await ReplyAsync("", false, embedded.Build());
        }
    }
}
