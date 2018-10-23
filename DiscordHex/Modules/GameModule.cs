using Discord.Commands;
using DiscordHex.Services;
using System.Threading.Tasks;
using System;

namespace DiscordHex.Modules
{
    [Name("Adventure Game module")]
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        public GameSession GameSession { get; set; }

        [Command("gamestart")]
        [Summary("Start an adventure.")]
        public async Task StartGame()
        {
            TemporaryGameCleanup();
            var service = GetGameService(Context.Message.Author.Id, true);

            if (service.State == GameState.NotStarted)
            {
                service.GameOwner = Context.Message.Author.Id;
                service.State = GameState.Started;
                service.Context = Context;

                service.StartGame();
            }
        }

        [Command("gameopt"), Alias("opt")]
        public async Task SelectOption(string opt)
        {
            var service = GetGameService(Context.Message.Author.Id);

            if (service == null)
            {
                await ReplyAsync("You have no games running");
            }
            else if (Context.Message.Author.Id == service.GameOwner 
                && service.State == GameState.Started 
                && !string.IsNullOrEmpty(opt))
            {
                int.TryParse(opt, out var res);
                if (res == 0) await ReplyAsync("That's not a valid option.");

                service.SelectOption(res);
            }
        }

        [Command("gameend")]
        [Summary("End your game.")]
        public async Task EndGame()
        {
            var service = GetGameService(Context.Message.Author.Id);
            if (service != null)
            {
                GameSession.ActiveGamesSessions.Remove(Context.Message.Author.Id);
                await ReplyAsync("Game ended");
            }
        }

        private GameService GetGameService(ulong userId, bool createNew = false)
        {
            GameService service;
            GameSession.ActiveGamesSessions.TryGetValue(Context.Message.Author.Id, out service);
            if (service == null && createNew)
            {
                service = new GameService();
                GameSession.ActiveGamesSessions.Add(Context.Message.Author.Id, service);
            }
            return service;
        }


        //tmp solution - fix -.-
        private void TemporaryGameCleanup()
        {
            foreach(var game in GameSession.ActiveGamesSessions)
            {
                if (game.Value.State == GameState.NotStarted)
                    GameSession.ActiveGamesSessions.Remove(game.Value.GameOwner);
            }
        }
    }
}
