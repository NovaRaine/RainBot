using DiscordHex.Services;
using System;
using System.Collections.Generic;

namespace DiscordHex.Core
{
    internal sealed class BotSettings
    {
        private static readonly BotSettings instance = new BotSettings();

        #region Members

        internal List<SpellEntity> Hexes { get; set; }
        internal List<SpellEntity> Buffs { get; set; }
        internal List<SpellEntity> DirectDamage { get; set; }
        internal List<ulong> AuthorizedWitches { get; set; }
        internal Random RandomNumber { get; private set; }
        internal Dictionary<string, string> CommandHelpTexts { get; set; }

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
