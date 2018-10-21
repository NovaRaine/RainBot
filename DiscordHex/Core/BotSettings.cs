using DiscordHex.Domain;
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
        internal Random RandomNumber { get; private set; }
        internal Dictionary<string, string> CommandHelpTexts { get; set; }
        internal string ConnectionString => "Data Source=c:\\temp\\hexer\\discordHexer.db;Version=3;";
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
