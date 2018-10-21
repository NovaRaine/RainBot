using Discord.Commands;
using DiscordHex.Services;
using System.Threading.Tasks;

namespace DiscordHex.Modules
{
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        public GameService GameService { get; set; }

        public GameModule(GameService gameService)
        {
            GameService = gameService;
        }

        [Command("gamestart")]
        public async Task StartGame()
        {
            if (GameService.State == GameState.NotStarted && Context.Channel.Name.Contains("bot-spam"))
            {
                GameService.GameOwner = Context.Message.Author.Id;
                GameService.State = GameState.Started;
                GameService.Context = Context;

                GameService.StartGame();
            }
        }

        [Command("gameopt"), Alias("opt")]
        public async Task SelectOption(string opt)
        {
            if (Context.Message.Author.Id == GameService.GameOwner 
                && GameService.State == GameState.Started 
                && !string.IsNullOrEmpty(opt))
            {
                int.TryParse(opt, out var res);
                if (res == 0) await ReplyAsync("That's not a valid option.");

                GameService.SelectOption(res);
            }
        }

        [Command("gameend")]
        public async Task EndGame()
        {
            if (Context.Message.Author.Id == GameService.GameOwner
                || Context.Message.Author.Id == 462658205009575946)
            {
                GameService.EndGame();
                await ReplyAsync("Game ended");
            }
        }
    }
}
