using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordHex.Services;
using System;
using System.IO;
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
        public async Task ServeTea(params IUser[] user)
        {
            if (user != null && user.Length > 0)
                await ReplyAsync($"{Context.Message.Author.Username} has served {user[0].Username} a cup of tea. Jolly good show!");
            else
                await ReplyAsync($"{Context.Message.Author.Username} has served tea. Jolly good show!");
        }

        [Command("cat")]
        public async Task CatAsync(params IUser[] users)
        {
            var text = users != null && users.Length > 0
                ? $"{Context.Message.Author.Username} tosses a pissed of cat at {users[0].Username}\n{users[0].Username} runs around in panic, screaming for sweet sweet mercy"
                : $"{Context.Message.Author.Username} gets jumped by a swarm of angry cats! Oh no! :o";

            var stream = await RandomCatPictureService.GetCatPictureAsync();
            stream.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(stream, "cat.png", text);
        }

        [Command("hex")]
        public async Task CastHex(params IUser[] users)
        {
            var embedded = HexingService.CastHex(users, Context.Message.MentionedRoles, Discord.CurrentUser.Id);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("esuna")]
        public async Task CastEsuna(params IUser[] users)
        {
            var embedded = FfxivSpellService.CastEsuna(users, Context.Message.MentionedRoles, Context.Message.Author.Username);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("love")]
        public async Task LoveSomeone(params IUser[] users)
        {
            var embedded = CommonCommands.LoveSomeone(users, Context.Message.MentionedRoles, Context.Message.Author.Username);
            await ReplyAsync("", false, embedded.Build());
        }
    }
}
