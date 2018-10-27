using Discord;
using Discord.Commands;
using DiscordHex.Services;
using System;
using System.Threading.Tasks;

namespace DiscordHex.Modules
{
    public class OwnerModule : ModuleBase<SocketCommandContext>
    {
        public CommonCommands CommonCommands { get; set; }
        public RandomPictureService RandomPictureService { get; set; }

        [Command("version")]
        [Alias("ver")]
        [RequireOwner]
        public Task PingAsync()
            => ReplyAsync(Environment.GetEnvironmentVariable("Version"));

        [Command("rndgif")]
        [RequireOwner]
        public async Task RndGif(params string[] message)
        {

            var url = RandomPictureService.GetRandomGiphyByTag(message[0]);
            if (!string.IsNullOrEmpty(url))
            {
                var embedded = new EmbedBuilder();
                embedded.ImageUrl = url;
                await ReplyAsync("", false, embedded.Build());
                return;
            }

            await ReplyAsync("no results :unamused:");
        }

        [Command("info")]
        [RequireOwner]
        public async Task Info(params string[] message)
        {
            await ReplyAsync(CommonCommands.ProcessInfo());
        }
    }
}
