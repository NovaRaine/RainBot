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
        public GameState State { get; set; } = GameState.NotStarted;
        public SocketCommandContext Context { get; set; }
        public TreeItem<GameLocationEntity> CurrenntLocation { get; set; }
        
        private IEnumerable<TreeItem<GameLocationEntity>> StoryArc { get; set; }

        public GameRepository _gameRepository { get; set; }

        public GameService()
        {
            _gameRepository = new GameRepository();
        }

        internal void SelectOption(int opt)
        {
            if (CurrenntLocation.Children.Any())
            {
                try
                {
                    CurrenntLocation = CurrenntLocation.Children.ToList()[opt-1];
                    SendReply();
                }
                catch
                {
                    Context.Message.Channel.SendMessageAsync("Sorry, that's not a valid option.");
                }
            }
        }

        internal void StartGame()
        {
            Context.Message.Channel.SendMessageAsync("Thanks for playing :)\nYou will get a description, followed by some options.\nSelect options by typing >>opt [option number]\n\n");
            StoryArc = GetStoryArc();
            if (StoryArc.Any())
            {
                CurrenntLocation = StoryArc.FirstOrDefault();
            }
            SendReply();
        }

        private void SendReply()
        {
            var s = new StringBuilder();
            var e = new EmbedBuilder();

            s.AppendLine(CurrenntLocation.Item.Description);
            if (CurrenntLocation.Children.Any())
            {
                s.AppendLine("\nOptions:");
                var i = 1;
                foreach (var opt in CurrenntLocation.Children)
                {
                    s.AppendLine($"{i} - {opt.Item.OptionTitle}");
                    i++;
                }
            }
            else // no more locations, game ended
            {
                EndGame();
            }

            e.Description = s.ToString();
            Context.Message.Channel.SendMessageAsync("", false, e.Build());
        }

        private IEnumerable<TreeItem<GameLocationEntity>> GetStoryArc()
        {
            var arc = _gameRepository.GetStoryArc(1);

            return arc;
        }

        public void EndGame()
        {
            GameOwner = 0;
            State = GameState.NotStarted;
            CurrenntLocation = null;
        }
    }

    public enum GameState
    {
        Started,
        NotStarted
    }
}
