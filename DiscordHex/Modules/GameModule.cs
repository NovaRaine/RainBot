using Discord.Commands;
using DiscordHex.Services;
using System.Threading.Tasks;
using DiscordHex.Data;
using DiscordHex.Domain;

namespace DiscordHex.Modules
{
    [Name("Adventure Game module")]
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        public GameSession GameSession { get; set; }

        [Command("gamestart")]
        [Summary("Start an adventure.")]
        [Remarks("No special usage.")]
        public async Task StartGame()
        {
            TemporaryGameCleanup();
            var service = GetGameService(true);

            if (service.State == GameStateEnum.NotRunning)
            {
                service.GameOwner = Context.Message.Author.Id;
                service.State = GameStateEnum.Started;
                service.Context = Context;

                service.StartGame();
            }
        }

        [Command("gameopt"), Alias("opt")]
        [Remarks("opt [number]")]
        public async Task SelectOption(string opt)
        {
            var service = GetGameService();

            if (service == null)
            {
                await ReplyAsync("You have no games running");
            }
            else if (Context.Message.Author.Id == service.GameOwner 
                && service.State == GameStateEnum.Started 
                && !string.IsNullOrEmpty(opt))
            {
                int.TryParse(opt, out var res);
                if (res == 0) await ReplyAsync("That's not a valid option.");

                service.SelectOption(res);
            }
        }

        [Command("gameend")]
        [Summary("End your game.")]
        [Remarks("No special usage.")]
        public async Task EndGame()
        {
            var service = GetGameService();
            if (service != null)
            {
                GameSession.ActiveGamesSessions.Remove(Context.Message.Author.Id);
                await ReplyAsync("Game ended");
            }
        }

        private GameService GetGameService(bool createNew = false)
        {
            GameSession.ActiveGamesSessions.TryGetValue(Context.Message.Author.Id, out var service);
            if (service == null && createNew)
            {
                service = new GameService(new BotContext());
                GameSession.ActiveGamesSessions.Add(Context.Message.Author.Id, service);
            }
            return service;
        }


        //tmp solution - fix -.-
        private void TemporaryGameCleanup()
        {
            foreach(var game in GameSession.ActiveGamesSessions)
            {
                if (game.Value.State == GameStateEnum.NotRunning)
                    GameSession.ActiveGamesSessions.Remove(game.Value.GameOwner);
            }
        }
    }
}
