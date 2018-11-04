using DiscordHex.Domain;
using System;

namespace DiscordHex.Core
{
    internal sealed class BotSettings
    {
        private static readonly BotSettings instance = new BotSettings();

        #region Members
                
        internal Random RandomNumber { get; private set; }
        internal BotConfigurationEntity Config { get; set; }
        #endregion

        static BotSettings()
        {
        }

        private BotSettings()
        {
            RandomNumber = new Random();
        }

        public static BotSettings Instance 
        {
            get
            {
                return instance;
            }
        }
    }
}
