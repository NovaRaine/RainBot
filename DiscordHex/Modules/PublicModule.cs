using Discord.Commands;
using Discord.WebSocket;
using DiscordHex.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordHex.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        public RandomCatPictureService RandomCatPictureService { get; set; }
        public HexingService HexingService { get; set; }
        public FfxivSpellService FfxivSpellService { get; set; }
        public CommonCommands CommonCommands { get; set; }
        public DiscordSocketClient Discord { get; set; }

        internal PublicModule(RandomCatPictureService pictureService, HexingService hexingService, DiscordSocketClient discord)
        {
            RandomCatPictureService = pictureService;
            HexingService = hexingService;
            Discord = discord;
        }

        [Command("version")]
        [Alias("ver")]
        public Task PingAsync()
            => ReplyAsync(Environment.GetEnvironmentVariable("Version"));

        [Command("tea")]
        public async Task ServeTea(params string[] message)
        {
            if (Context.Message.MentionedUsers.Count > 0)
                await ReplyAsync($"{Context.Message.Author.Username} has served {Context.Message.MentionedUsers.First().Username} a cup of tea. Jolly good show!");
            else
                await ReplyAsync($"{Context.Message.Author.Username} has served tea. Jolly good show!");
        }

        [Command("cat")]
        public async Task CatAsync(params string[] message)
        {
            var text = Context.Message.MentionedUsers.Count > 0
                ? $"{Context.Message.Author.Username} gifts a kitteh to {Context.Message.MentionedUsers.First().Username}! Hurray :D"
                : $"Awwwwww.. look, a kitty has come to visit";

            var stream = await RandomCatPictureService.GetCatPictureAsync();
            stream.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(stream, "cat.png", text);
        }

        [Command("hex")]
        public async Task CastHex(params string[] message)
        {
            var embedded = HexingService.CastHex(Context.Message.MentionedUsers, Context.Message.MentionedRoles, Discord.CurrentUser.Id);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("esuna")]
        public async Task CastEsuna(params string[] message)
        {
            var embedded = FfxivSpellService.CastEsuna(Context.Message.MentionedUsers, Context.Message.MentionedRoles, Context.Message.Author.Username);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("love")]
        public async Task LoveSomeone(params string[] message)
        {
            var embedded = CommonCommands.LoveSomeone(Context.Message.MentionedUsers, Context.Message.MentionedRoles, Context.Message.Author.Username);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("info")]
        public async Task Info(params string[] message)
        {
            await ReplyAsync(CommonCommands.ProcessInfo());
        }
    }
}
