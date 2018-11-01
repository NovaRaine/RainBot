using Discord;
using Discord.Commands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordHex.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        public CommandService CommandService { get; set; }

        [Command("help")]
        [Alias("h", "?")]
        [Summary("Shows the help info. You just used it you dummy.")]
        [Remarks("help | help [Command name]")]
        public async Task Help()
        {
            await Context.Message.Author.SendMessageAsync("Thank you for calling RainBot Customer Support.\nHere at RainBot Enterprises, we love all our customers. And especially you! <3\n\nHere's a list of what I can do.");
            var e = new EmbedBuilder();

            foreach (var module in CommandService.Modules)
            {
                foreach (var command in module.Commands.Where(x => !string.IsNullOrEmpty(x.Summary)).OrderBy(x => x.Name))
                {
                    var aliases = command.Aliases.Count > 1 ? string.Join(", ", command.Aliases) : string.Empty;

                    var extra = "";
                    if (!string.IsNullOrEmpty(aliases))
                        extra = $"\nAlias: {aliases}";

                    e.AddField(command.Name, command.Summary + extra);
                }
            }

            await Context.Message.Author.SendMessageAsync("", false, e.Build());
        }

        [Command("help")]
        [Alias("h", "?")]
        public async Task Help(string command)
        {
            var res = CommandService.Search(Context, command);
            if (res.IsSuccess && res.Commands.Count > 0)
            {
                var cmd = res.Commands.First().Command;

                var emb = new EmbedBuilder();
                var sb = new StringBuilder();

                sb.AppendLine($"Command: {cmd.Name}");
                sb.AppendLine($"Summary: {cmd.Summary}");
                sb.AppendLine($"Usage: {cmd.Remarks}");

                emb.Description = sb.ToString();
                await Context.Message.Author.SendMessageAsync("", false, emb.Build());
                return;
            }

            await Context.Message.Author.SendMessageAsync("No command with that name found.");
        }
    }
}
