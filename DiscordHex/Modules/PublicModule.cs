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
        public RandomPictureService RandomPictureService { get; set; }
        public HexingService HexingService { get; set; }
        public FfxivSpellService FfxivSpellService { get; set; }
        public CommonCommands CommonCommands { get; set; }
        public DiscordSocketClient Discord { get; set; }
        public CommandService CommandService { get; set; }

        internal PublicModule(RandomPictureService pictureService, HexingService hexingService, DiscordSocketClient discord, CommandService commandService)
        {
            RandomPictureService = pictureService;
            HexingService = hexingService;
            Discord = discord;
        }

        [Command("version")]
        [Alias("ver")]
        [Summary("Show bot version")]
        public Task PingAsync()
            => ReplyAsync(Environment.GetEnvironmentVariable("Version"));

        [Command("serve")]
        [Summary("Serve something to another person. Whatever. Might be tea, might be revenge.")]
        public async Task Serve(IUser user, params string[] message)
        {
            var msg = (string.Join(' ', message));
            if (string.IsNullOrEmpty(msg))
                msg = "nothing.";

            await ReplyAsync($"{user.Username}, you have been served {msg}");

        }

        [Command("serve")]
        [Alias("tea")]
        public async Task Serve(params string[] message)
        {
            var msg = (string.Join(' ', message));
            if (string.IsNullOrEmpty(msg))
                msg = $"nothing. {Context.Message.Author.Username} is a greedy one -.-";

            if (Context.Message.MentionedUsers.Count > 0)
                await ReplyAsync($"{Context.Message.MentionedUsers.First().Username}, you have been served {msg}");
            else
                await ReplyAsync($"{Context.Message.Author.Username} has served {msg}");

        }

        [Command("cat")]
        [Alias("kitty", "radomcat")]
        [Summary("Get a radom cat in your channel! An important part of the internet.")]
        public async Task CatAsync(params string[] message)
        {
            var text = Context.Message.MentionedUsers.Count > 0
                ? $"{Context.Message.Author.Username} gifts a kitteh to {Context.Message.MentionedUsers.First().Username}! Hurray :D"
                : $"Awwwwww.. look, a kitty has come to visit";

            var stream = await RandomPictureService.GetPictureAsync("https://cataas.com/cat");
            stream.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(stream, "CatsAreEvil.png", text);
        }

        [Command("dog")]
        [Alias("puppy", "doggo", "pupper")]
        [Summary("Random dogs. Everyone loves random dogs.")]
        public async Task Dog(params string[] message)
        {
            var text = Context.Message.MentionedUsers.Count > 0
                ? $"{Context.Message.MentionedUsers.First().Username}! You got a doggie from {Context.Message.Author.Username} :>"
                : $"Awwwwww.. look, a dawg has come to visit";

            var stream = await RandomPictureService.GetPictureAsync("https://www.randomdoggiegenerator.com/randomdoggie.php");
            stream.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(stream, "DeniLikesDogs.png", text);
        }

        [Command("bunny")]
        [Alias("bun", "bunbun", "bunneh")]
        [Summary("For when cats and dogs are just not good enough.")]
        public async Task Bunny(params string[] message)
        {
            var text = Context.Message.MentionedUsers.Count > 0
                ? $"{Context.Message.MentionedUsers.First().Username}! A bunneh from {Context.Message.Author.Username} :3"
                : $"Bunbun!!";

            var url = RandomPictureService.GetRandomGiphyByTag("bunny");
            if (!string.IsNullOrEmpty(url))
            {
                var embedded = new EmbedBuilder();
                embedded.Description = text;
                embedded.ImageUrl = url;
                await ReplyAsync("", false, embedded.Build());
                return;
            }

            await ReplyAsync("The bunnies are hiding :/");
        }

        [Command("chocolate")]
        [Alias("choco")]
        [Summary("Eat some chocolate, or share with someone else.")]
        public async Task GetChocolate(params string[] message)
        {
            var embedded = new EmbedBuilder();
            if (Context.Message.MentionedUsers.Count > 0)
            {
                embedded.Description = $"{Context.Message.Author.Username} throws a bunch of chocolate at {Context.Message.MentionedUsers.First().Username}.. Why?";
                embedded.ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/70/Chocolate_%28blue_background%29.jpg";
            }
            else
            {
                embedded.Description = $"{Context.Message.Author.Username} noms some chocolate. Without sharing..";
            }
            
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("chop")]
        [Alias("chopchop")]
        [Summary("Chop!")]
        public async Task Chop(params string[] message)
        {
            var embedded = new EmbedBuilder();
            embedded.ImageUrl = "https://cdn.discordapp.com/attachments/461009638922649610/482247931831910411/aa7eb8e.gif";

            if (Context.Message.MentionedUsers.Count > 0)
            {
                embedded.Description = $"Chop! Chopchop, {Context.Message.MentionedUsers.First().Username}!";
            }
            else
            {
                embedded.Description = "Chop! Chopchop!";
            }

            await ReplyAsync("", false, embedded.Build());
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
        [Summary("Shows the help info. You just used it you dummy.")]
        public async Task Help(params string[] message)
        {
            var embedded = new EmbedBuilder();
            embedded.WithTitle("Here's a list of what I can do.");
            foreach (var module in CommandService.Modules)
            {
                foreach(var command in module.Commands.Where(x => !string.IsNullOrEmpty(x.Summary)))
                {
                    var aliases = command.Aliases.Count > 1 ? string.Join(", ", command.Aliases) : string.Empty;
                    var text = command.Summary;
                    if (!string.IsNullOrEmpty(aliases))
                        text = text + "\n\tAlias: " + aliases;

                    embedded.AddField(command.Name, text);
                }                    
            }

            await Context.Message.Author.SendMessageAsync("Thank you for calling RainBot Customer Support.\nHere at RainBot Enterprises, we love all our customers. And especially you! <3");
            await Context.Message.Author.SendMessageAsync("", false, embedded.Build());
        }
    }
}
