using Discord;
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
        public CommandService CommandService { get; set; }

        internal PublicModule(RandomCatPictureService pictureService, HexingService hexingService, DiscordSocketClient discord, CommandService commandService)
        {
            RandomCatPictureService = pictureService;
            HexingService = hexingService;
            Discord = discord;
        }

        [Command("version")]
        [Alias("ver")]
        [Summary("Show bot version")]
        public Task PingAsync()
            => ReplyAsync(Environment.GetEnvironmentVariable("Version"));

        [Command("tea")]
        [Summary("Serve a cup of tea to someone you like.")]
        public async Task ServeTea(params string[] message)
        {
            if (Context.Message.MentionedUsers.Count > 0)
                await ReplyAsync($"{Context.Message.Author.Username} has served {Context.Message.MentionedUsers.First().Username} a cup of tea. Jolly good show!");
            else
                await ReplyAsync($"{Context.Message.Author.Username} has served tea. Jolly good show!");
        }

        [Command("cat")]
        [Alias("kitty", "radomcat")]
        [Summary("Get a radom cat in your channel! An important part of the internet.")]
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
        [Alias("curse")]
        [Summary("Cast a hex on your nemesis, or just on whoever.")]
        public async Task CastHex(params string[] message)
        {
            var embedded = HexingService.CastHex(Context.Message.MentionedUsers, Context.Message.MentionedRoles, Discord.CurrentUser.Id);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("esuna")]
        [Summary("Cast Esuna on someone to remove a negative effect. Possibly..")]
        public async Task CastEsuna(params string[] message)
        {
            var embedded = FfxivSpellService.CastEsuna(Context.Message.MentionedUsers, Context.Message.MentionedRoles, Context.Message.Author.Username);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("love")]
        [Alias("witchylove", "hate")]
        [Summary("Show someone how that you love them very much!")]
        public async Task LoveSomeone(params string[] message)
        {
            var embedded = CommonCommands.LoveSomeone(Context.Message.MentionedUsers, Context.Message.MentionedRoles, Context.Message.Author.Username);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("info")]
        [Summary("Just some random system info")]
        public async Task Info(params string[] message)
        {
            await ReplyAsync(CommonCommands.ProcessInfo());
        }

        [Command("help")]
        [Alias("h")]
        [Summary("Shows this thing. you just used it you dummy.")]
        public async Task Help()
        {
            var embedded = new EmbedBuilder();
            foreach(var module in CommandService.Modules)
            {
                foreach(var command in module.Commands)
                {
                    var aliases = command.Aliases.Count > 1 ? string.Join(", ", command.Aliases) : string.Empty;
                    var text = string.IsNullOrEmpty(command.Summary) ? "No summary." : command.Summary;
                    if (!string.IsNullOrEmpty(aliases))
                        text = text + "\n\tAlias: " + aliases;

                    embedded.AddField(command.Name, text);
                }                    
            }

            await Context.Message.Author.SendMessageAsync("Help incomming!\nHere's a list of my commands :)");
            await Context.Message.Author.SendMessageAsync("", false, embedded.Build());
        }
    }
}
