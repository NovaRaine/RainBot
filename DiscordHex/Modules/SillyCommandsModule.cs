using Discord;
using Discord.Commands;
using DiscordHex.Core;
using DiscordHex.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordHex.Modules
{
    public class SillyCommandsModule : ModuleBase<SocketCommandContext>
    {
        public RandomPictureService RandomPictureService { get; set; }
        private List<string> words;

        public SillyCommandsModule()
        {
            words = new List<string>
            {
                "Booty booty rump rump, {0} got a pretty butt bump",
                "{0} likes butts and cannot lie.",
                "Did you try buffing?",
                "Butts.. That's all.",
                "Butty butty bum bum",
                "You pervert!"
            };
        }

        [Command("butt")]
        [Summary("Bum bum")]
        public async Task Butt(params string[] message)
        {
            var emb = new EmbedBuilder();
            var text = words.ElementAt(BotSettings.Instance.RandomNumber.Next(0, words.Count));

            text = string.Format(text, Context.Message.Author.Username);

            emb.Description = text;

            await ReplyAsync("", false, emb.Build());
        }

        [Command("hiss")]
        [Summary("Hiss. It's fun at times.")]
        [Remarks("hiss | hiss @user")]
        public async Task Hissss(params string[] message)
        {
            var emb = new EmbedBuilder();

            var text = Context.Message.MentionedUsers.Count > 0
                ? $"{Context.Message.Author.Username} hisses at {Context.Message.MentionedUsers.First().Username}!"
                : $"{Context.Message.Author.Username} hisses!";

            var url = RandomPictureService.GetRandomGiphyByTag("hiss");
            if (!string.IsNullOrEmpty(url))
                emb.ImageUrl = url;

            emb.Description = text;

            await ReplyAsync("", false, emb.Build());
        }

        [Command("chop")]
        [Alias("chopchop")]
        [Summary("Chop!")]
        [Remarks("chop | chop @Atumra (HAMSTER). Is bitten. ")]
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
    }
}
