using System;
using System.Collections.Generic;

namespace DiscordHex.core
{
    internal sealed class BotSettings
    {
        private static readonly BotSettings instance = new BotSettings();

        #region Members

        internal string Token { get; set; }
        internal string Prefix { get; set; } = ">>";
        internal string ImageBasePath { get; set; }
        internal bool AllowAll { get; set; }
        internal ulong SelfId { get; set; }
        internal Dictionary<string, string> Hexes { get; set; }
        internal List<ulong> ApprovedWitches { get; set; }
        internal Random RandomNumber { get; private set; }

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
