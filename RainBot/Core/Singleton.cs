using System;

namespace RainBot.Core
{
    internal sealed class Singleton
    {
        #region Members

        internal Random RandomNumber { get; private set; }

        #endregion Members

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