using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RainBot.Core;
using RainBot.Services;

namespace RainBot.Modules
{
    public class OwnerModule : ModuleBase<SocketCommandContext>
    {
        public CommonCommands CommonCommands { get; set; }
        public DiscordSocketClient Discord { get; set; }
        public AdministrationService AdministrationService { get; set; }

        public OwnerModule(DiscordSocketClient discord)
        {
            Discord = discord;
        }

        [Command("setprefix")]
        [RequireOwner]
        public async Task SetPrefix(string prefix)
        {
            AdministrationService.SetPrefix(prefix, (Context.Message.Channel as SocketTextChannel).Guild.Id);
            await ReplyAsync($"New prefix: {prefix}");
        }

        [Command("version")]
        [Alias("ver")]
        [RequireOwner]
        public Task Version()
            => ReplyAsync(Environment.GetEnvironmentVariable("Version"));

        [Command("info")]
        [RequireOwner]
        public async Task Info(params string[] message)
        {
            await ReplyAsync(CommonCommands.ProcessInfo());
        }

        [Command("userinfo")]
        [RequireOwner]
        public async Task UserInfoAsync(IUser user = null)
        {
            user = user ?? Context.User;

            var s = new StringBuilder()
                .AppendLine($"Name: {user.Username}")
                .AppendLine($"ID: {user.Id}")
                .AppendLine($"Created: {user.CreatedAt}")
                .AppendLine($"IsBot: {user.IsBot}");

            await Context.Message.Author.SendMessageAsync(s.ToString());
        }

        [Command("getguilds"), Alias("gg")]
        [RequireOwner]
        public async Task GetGuilds(string args = "")
        {
            if (args.ToLower() == "list")
            {
                var s = new StringBuilder();
                foreach (var g in Discord.Guilds)
                {
                    s.AppendLine($"Guild: {g.Name} ({g.Id})");
                    s.AppendLine($"Owner: {g.Owner.Username} ({g.Owner.Id})");
                    s.AppendLine();
                }

                await ReplyAsync(s.ToString());
            }
            else
                await ReplyAsync($"Connected guilds: {Discord.Guilds.Count}");
        }

        [Command("getchannels")]
        [RequireOwner]
        public async Task GetChannels(ulong guildid)
        {
            var guild = Discord.GetGuild(guildid);

            if (guild == null)
            {
                await ReplyAsync("Sorry mom, I found no guild with that ID");
                return;
            }

            foreach (var c in guild.Channels)
            {
                await ReplyAsync($"Channel: {c.Name} ({c.Id})");
            }

            await ReplyAsync("done");
        }

        [Command("send")]
        [RequireOwner]
        public async Task Send(ulong guildid, ulong channelId, params string[] msg)
        {
            var guild = Discord.GetGuild(guildid);
            
            if (guild == null)
            {
                await ReplyAsync("Sorry mom, I found no guild with that ID");
                return;
            }

            BotConfig.SetValue("LastChannelId", $"{channelId}");
            BotConfig.SetValue("LastGuildId", $"{guildid}");

            await guild.GetTextChannel(channelId).SendMessageAsync(string.Join(" ", msg));
            await ReplyAsync("Done! :)");
        }

        [Command("rsend")]
        [RequireOwner]
        public async Task Rsend(params string[] msg)
        {
            var cid = BotConfig.GetValue("LastChannelId");
            var gid = BotConfig.GetValue("LastGuildId");

            if (string.IsNullOrEmpty(cid) || string.IsNullOrEmpty(gid))
            {
                await ReplyAsync("Last channel or guild not set. Abort!");
                return;
            }

            var lastChannel = Convert.ToUInt64(cid);
            var lastGuild = Convert.ToUInt64(gid);

            var guild = Discord.GetGuild(lastGuild);

            if (guild == null)
            {
                await ReplyAsync("Sorry mom, I found no guild with that ID");
                return;
            }

            var channel = guild.GetTextChannel(lastChannel);

            if (guild == null)
            {
                await ReplyAsync("Sorry mom, I found no chennel with that ID");
                return;
            }

            await channel.SendMessageAsync(string.Join(" ", msg));
            await ReplyAsync("Done! :)");
        }
    }
}