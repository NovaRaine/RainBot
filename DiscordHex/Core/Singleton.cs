using System;

namespace DiscordHex.Core
{
    internal sealed class Singleton
    {
        #region Members

        internal Random RandomNumber { get; private set; }
        #endregion

        static Singleton()
        {
        }

        private Singleton()
        {
            RandomNumber = new Random();
        }

        public static Singleton I { get; } = new Singleton();
    }
}