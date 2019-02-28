using System.Collections.Generic;

namespace RainBot.Services
{
    public class GameSession
    {
        public Dictionary<ulong, GameService> ActiveGamesSessions;

        public GameSession()
        {
            ActiveGamesSessions = new Dictionary<ulong, GameService>();
        }
    }
}