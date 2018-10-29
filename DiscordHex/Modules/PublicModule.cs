using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordHex.Services;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordHex.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        public RandomPictureService RandomPictureService { get; set; }
        public DiscordSocketClient Discord { get; set; }
        public CommonCommands CommonCommands { get; set; }

        internal PublicModule(RandomPictureService pictureService, DiscordSocketClient discord, CommandService commandService)
        {
            RandomPictureService = pictureService;
            Discord = discord;
        }

        [Command("serve")]
        [Summary("Serve something to another person. Whatever. Might be tea, might be revenge.")]
        [Remarks("serve [text] | serve @user [text]")]
        public async Task Serve(IUser user, params string[] message)
        {
            var msg = (string.Join(' ', message));
            if (string.IsNullOrEmpty(msg))
                msg = "nothing.";

            await ReplyAsync($"{user.Username}, you have been served {msg}");
        }

        [Command("serve")]
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
        [Remarks("cat | cat @user")]
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
        [Remarks("dog | dog @user")]
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
        [Remarks("bun | bun @user")]
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

        [Command("mokepon")]
        [Alias("pokemon", "pokeyman", "pokkie")]
        [Summary("Mokepons!")]
        [Remarks("No special usage.")]
        public async Task Mokepon(params string[] message)
        {
            var url = RandomPictureService.GetRandomGiphyByTag("pokemon");
            if (!string.IsNullOrEmpty(url))
            {
                var embedded = new EmbedBuilder();
                embedded.Description = "Pokeymun!";
                embedded.ImageUrl = url;
                await ReplyAsync("", false, embedded.Build());
                return;
            }

            await ReplyAsync("Something happened.. sorry -.-");
        }

        [Command("chocolate")]
        [Alias("choco")]
        [Summary("Eat some chocolate, or share with someone else.")]
        [Remarks("choco | choco @user")]
        public async Task GetChocolate(params string[] message)
        {
            var embedded = new EmbedBuilder();
            if (Context.Message.MentionedUsers.Count > 0)
            {
                embedded.Description = $"{Context.Message.Author.Username} throws a bunch of chocolate at {Context.Message.MentionedUsers.First().Username}";
                embedded.ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/70/Chocolate_%28blue_background%29.jpg";
            }
            else
            {
                embedded.Description = $"{Context.Message.Author.Username} noms some chocolate. Without sharing..";
            }
            
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("love")]
        [Alias("witchylove", "hate")]
        [Summary("Show someone how that you love them very much!")]
        [Remarks("love | love @user")]
        public async Task LoveSomeone(params string[] message)
        {
            var embedded = CommonCommands.LoveSomeone(Context.Message.MentionedUsers, Context.Message.MentionedRoles, Context.Message.Author.Username);
            await ReplyAsync("", false, embedded.Build());
        }
    }
}
