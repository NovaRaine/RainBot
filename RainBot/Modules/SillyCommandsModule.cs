﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RainBot.Core;
using RainBot.Services;

namespace RainBot.Modules
{
    public class SillyCommandsModule : ModuleBase<SocketCommandContext>
    {
        public RandomPictureService RandomPictureService { get; set; }
        public CommonCommands CommonCommands { get; set; }
        public MoodService MoodService { get; set; }
        private readonly List<string> _words;

        public SillyCommandsModule()
        {
            _words = new List<string>
            {
                "Booty booty rump rump, {0} got a pretty butt bump",
                "{0} likes butts and cannot lie.",
                "Butts.. That's all.",
                "Butty butty bum bum",
                "You pervert!"
            };
        }

        [Command("nou")]
        public async Task NoU()
        {
            await ReplyAsync("Three days and nights it had taken them. Through uncharted and unseen jungle.\nTen had set out, but now only six remained. Two more would be lost before they returned home. \n\nAll to reach this.The temple that shouldn't exist. The inscription over the entrance simply read:\n'no u'");
        }

        [Command("nova")]
        [Alias("mommy")]
        [Summary("About Nova")]
        public async Task AboutNova()
        {
            var emb = new EmbedBuilder();

            emb.ImageUrl = "https://storage.googleapis.com/gsposts/Div/Bunnydragon.jpg";
            emb.Description = BotConfig.GetValue("AboutNova");

            await ReplyAsync("", false, emb.Build());
        }

        [Command("butt")]
        [Summary("Bum bum")]
        public async Task Butt(params string[] message)
        {
            var emb = new EmbedBuilder();
            var text = _words.ElementAt(Singleton.I.RandomNumber.Next(0, _words.Count));

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
        [Remarks("chop | chop @user")]
        public async Task Chop(params string[] message)
        {
            var embedded = new EmbedBuilder();
            embedded.ImageUrl = "https://cdn.discordapp.com/attachments/461009638922649610/482247931831910411/aa7eb8e.gif";

            embedded.Description = Context.Message.MentionedUsers.Count > 0
                ? $"Chop! Chopchop, {Context.Message.MentionedUsers.First().Username}!"
                : "Chop! Chopchop!";

            await ReplyAsync("", false, embedded.Build());
        }

        [Command("eureka")]
        [Summary("Everyone's favourite")]
        public async Task Eureka(params string[] message)
        {
            var emb = CommonCommands.Eureka();
            await ReplyAsync("", false, emb);
            MoodService.SendMoodReply(Context);
        }

        [Command("test")]
        public async Task Test()
        {
            await ReplyAsync("test");
            MoodService.SendMoodReply(Context);
        }
    }
}