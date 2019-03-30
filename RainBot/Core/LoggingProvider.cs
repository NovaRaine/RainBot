using Serilog;
using Serilog.Core;

namespace RainBot.Core
{
    public static class LoggingProvider
    {
        public static Logger GetLogger()
        {
            return GetConsoleLogger();
        }

        private static Logger GetConsoleLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}