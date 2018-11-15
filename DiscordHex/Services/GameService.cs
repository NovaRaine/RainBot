using Discord;
using Discord.Commands;
using DiscordHex.Data;
using DiscordHex.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordHex.Services
{
    public class GameService
    {
        public ulong GameOwner { get; set; }
        public GameStateEnum State { get; set; } = GameStateEnum.NotRunning;
        public SocketCommandContext Context { get; set; }
        public TreeItem<GameLocationEntity> CurrentLocation { get; set; }
        private IEnumerable<TreeItem<GameLocationEntity>> StoryArc { get; set; }
        private readonly GameRepository _gameRepository;

        public GameService()
        {
            _gameRepository = new GameRepository();
        }

        internal void SelectOption(int opt)
        {
            if (CurrentLocation.Children.Any())
            {
                try
                {
                    CurrentLocation = CurrentLocation.Children.ToList()[opt-1];
                    SendReply();
                }
                catch
                {
                    Context.Message.Channel.SendMessageAsync("Sorry, that's not a valid option.");
                }
            }
        }

        internal async void StartGame()
        {
            await Context.Message.Author.SendMessageAsync("Thanks for playing :)\nYou will get a description, followed by some options.\nSelect options by typing >>opt [option number]\n\n");
            StoryArc = GetStoryArc();
            CurrentLocation = StoryArc.FirstOrDefault();

            SendReply();
        }

        private async void SendReply()
        {
            var s = new StringBuilder();

            s.AppendLine(CurrentLocation.Item.Description);
            if (CurrentLocation.Children.Any())
            {
                s.AppendLine("\nOptions:");
                var i = 1;
                foreach (var opt in CurrentLocation.Children)
                {
                    s.AppendLine($"{i} - {opt.Item.OptionTitle}");
                    i++;
                }
            }
            else // no more locations, game ended
            {
                EndGame();
            }

            var msg = new EmbedBuilder()
                .WithDescription(s.ToString())
                .Build();

            await Context.Message.Author.SendMessageAsync("", false, msg);
        }

        private IEnumerable<TreeItem<GameLocationEntity>> GetStoryArc()
        {
            var arc = _gameRepository.GetStoryArc(1); // only one story right now. fix when adding more

            return arc;
        }

        public void EndGame()
        {
            State = GameStateEnum.NotRunning;
        }
    }

    
}
