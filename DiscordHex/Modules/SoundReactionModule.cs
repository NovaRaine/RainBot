using Discord;
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

        [Command("nap")]
        [Summary("Take a nap. You probably need it.")]
        [Remarks("nap")]
        public async Task NapSounds(params string[] message)
        {
            var embedded = SoundReactionService.GetRandomNap();
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("gs")]
        [Alias("gaysounds")]
        [Summary("Show your gayness in a react image.")]
        [Remarks("gs [search word] example: gs sad | show all with gs list")]
        public async Task GaySounds(params string [] type)
        {
            if (type.Length == 0)
                return;

            var embedded = SoundReactionService.GetGaySounds(type[0]);

            if (embedded.Title == "List")
            {
                await Context.Message.Author.SendMessageAsync("", false, embedded.Build());
                return;
            }

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

            if (embedded.Title == "List")
            {
                await Context.Message.Author.SendMessageAsync("", false, embedded.Build());
                return;
            }

            await ReplyAsync("", false, embedded.Build());
        }
    }
}
