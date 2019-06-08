using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using RainBot.Services;
using System.Linq;
using Discord;
using System.Text;

namespace RainBot.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        public RandomPictureService RandomPictureService { get; set; }
        public CommonCommands CommonCommands { get; set; }
        public DiscordSocketClient Discord { get; set; }

        internal PublicModule(RandomPictureService pictureService, DiscordSocketClient discord)
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

        [Command("rndgif")]
        [Alias("show", "giphy")]
        [Summary("Get a giphy by tag")]
        [Remarks("show [tag]")]
        public async Task RndGif(params string[] message)
        {
            var msg = string.Join(" ", message);
            var url = "";

            if (isNova(msg))
            {
                url = "https://storage.googleapis.com/gsposts/Div/Nova.gif";
            }
            else
            {
                url = RandomPictureService.GetRandomGiphyByTag(msg);
            }

            if (!string.IsNullOrEmpty(url))
            {
                var embedded = new EmbedBuilder();
                embedded.ImageUrl = url;
                await ReplyAsync("", false, embedded.Build());
                return;
            }

            await ReplyAsync("no results :unamused:");
        }

        private bool isNova(string msg)
        {
            return msg.ToLower() == "nova"
                || msg.ToLower() == "nova rain"
                || msg.ToLower() == "dragon mom";
        }

        [Command("cat")]
        [Alias("kitty", "radomcat")]
        [Summary("Get a radom cat in your channel! An important part of the internet.")]
        [Remarks("cat | cat @user")]
        public async Task Cat(params string[] message)
        {
            var text = Context.Message.MentionedUsers.Count > 0
                ? $"{Context.Message.Author.Username} gifts a kitteh to {Context.Message.MentionedUsers.First().Username}! Hurray :D"
                : $"Awwwwww.. look, a kitty has come to visit";

            var embed = GetRandomGiphyByTag(text, "cat");

            if (embed != null)
            {
                await ReplyAsync("", false, embed);
                return;
            }

            await ReplyAsync("The cats are hiding :unamused:");
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

            var embed = GetRandomGiphyByTag(text, "dog");

            if (embed != null)
            {
                await ReplyAsync("", false, embed);
                return;
            }

            await ReplyAsync("The dogs are hiding :/");
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

            var embed = GetRandomGiphyByTag(text, "bunny");

            if (embed != null) {
                await ReplyAsync("", false, embed);
                return;
            }

            await ReplyAsync("The bunnies are hiding :/");
        }

        private Embed GetRandomGiphyByTag(string embedDescription, string tag)
        {
            var url = RandomPictureService.GetRandomGiphyByTag(tag);

            if (!string.IsNullOrEmpty(url))
            {
                return new EmbedBuilder()
                    .WithDescription(embedDescription)
                    .WithUrl(url)
                    .Build();
            }

            return null;
        }

        [Command("mokepon")]
        [Alias("pokemon", "pokeyman", "pokkie")]
        [Summary("Mokepons!")]
        [Remarks("No special usage.")]
        public async Task Mokepon(params string[] message)
        {
            var embed = GetRandomGiphyByTag("Pokeymun!", "bunny");

            if (embed != null)
            {
                await ReplyAsync("", false, embed);
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
            if (Context.Message.MentionedUsers.Any(x => x.Id == 497491199918080002)
                || Context.Message.MentionedUsers.Any(x => x.Id == 498185985096679434))
                return; // bot targeted - handled elsewhere

            var embedded = CommonCommands.LoveSomeone(Context.Message.MentionedUsers, Context.Message.MentionedRoles, Context.Message.Author.Username);
            await ReplyAsync("", false, embedded.Build());
        }

        [Command("donate"), Alias("pay", "paypal")]
        [Summary("Donate to my author, Dragon Mom Nova")]
        public async Task Donate(params string[] message)
        {
            var msg = new EmbedBuilder()
                .WithTitle("Donate to Nova Rain!")
                .WithDescription("Hey! I know money is hard to get by.\nIf you feel like donating, just click the link and you'll be sent to PayPal :3")
                .WithUrl("https://www.paypal.me/novaraine")
                .Build();

            await ReplyAsync("", false, msg);
        }

        [Command("spunky")]
        public async Task Spunky()
        {
            if (Context.Message.Author.Id != 184013275841691648) return; // nah, you're not spunky

            var msg = new EmbedBuilder()
                .WithTitle("Spunky")
                .WithDescription($"Hi everyone!\nI'm Spunky :smile:")
                .WithThumbnailUrl(Context.Message.Author.GetAvatarUrl())
                .WithColor(new Color(100, 100, 200))
                .Build();

            await ReplyAsync("", false, msg);
        }
    }
}