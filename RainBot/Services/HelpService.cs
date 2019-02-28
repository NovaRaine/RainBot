using System.Linq;
using System.Text;
using Discord;
using Discord.Commands;

namespace RainBot.Services
{
    public class HelpService
    {
        public CommandService CommandService { get; set; }

        public HelpService(CommandService commandService)
        {
            CommandService = commandService;
        }

        public Embed GetHelp()
        {
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

            return e.Build();
        }

        public Embed GetCommandHelp(string command, SocketCommandContext context)
        {
            var res = CommandService.Search(context, command);
            if (res.IsSuccess && res.Commands.Count > 0)
            {
                var cmd = res.Commands.First().Command;

                var e = new EmbedBuilder();
                var sb = new StringBuilder();

                sb.AppendLine($"Command: {cmd.Name}");
                sb.AppendLine($"Summary: {cmd.Summary}");
                sb.AppendLine($"Usage: {cmd.Remarks}");

                e.Description = sb.ToString();

                return e.Build();
            }

            return null;
        }
    }
}