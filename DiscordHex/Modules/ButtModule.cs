using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DiscordHex.Core;

namespace DiscordHex.Modules
{
    public class ButtModule : ModuleBase<SocketCommandContext>
    {
        private List<string> words;

        public ButtModule()
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
    }
}
