using Serilog.Core;

namespace RainBot.Core
{
    public static class Log
    {
        public static Logger Logger { get; set; }

        public static void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        public static void Error(string message)
        {
            Logger.Error(message);
        }

        public static void Warning(string message)
        {
            Logger.Warning(message);
        }

        public static void Information(string message)
        {
            Logger.Information(message);
        }

        public static void Verbose(string message)
        {
            Logger.Verbose(message);
        }

        public static void Debug(string message)
        {
            Logger.Debug(message);
        }
    }
}