using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RainBot.Services;

namespace RainBot.Modules
{
    public class OwnerModule : ModuleBase<SocketCommandContext>
    {
        public CommonCommands CommonCommands { get; set; }
        public DiscordSocketClient Discord { get; set; }

        public OwnerModule(DiscordSocketClient discord)
        {
            Discord = discord;
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

            var s = new StringBuilder();
            foreach (var c in guild.Channels)
            {
                s.AppendLine($"Channel: {c.Name} ({c.Id})");
            }

            await ReplyAsync(s.ToString());
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

            await guild.GetTextChannel(channelId).SendMessageAsync(string.Join(" ", msg));
            await ReplyAsync("Done! :)");
        }
    }
}