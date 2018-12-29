using Discord;
using Discord.Commands;
using DiscordHex.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordHex.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        public CommandService CommandService { get; set; }
        public HelpService HelpService { get; set; }

        [Command("help")]
        [Alias("h", "?")]
        [Summary("Shows the help info. You just used it you dummy.")]
        [Remarks("help | help [Command name]")]
        public async Task Help()
        {
            var e = HelpService.GetHelp();
            await Context.Message.Author.SendMessageAsync("Thank you for calling RainBot Customer Support.\nHere at RainBot Enterprises, we love all our customers. And especially you! <3\n\nHere's a list of what I can do.");

            await Context.Message.Author.SendMessageAsync("", false, e);
        }

        [Command("help")]
        [Alias("h", "?")]
        public async Task Help(string command)
        {
            var e = HelpService.GetCommandHelp(command, Context);

            if (e == null)
                await Context.Message.Author.SendMessageAsync("No command with that name found.");

            await Context.Message.Author.SendMessageAsync("", false, e);
        }
    }
}
