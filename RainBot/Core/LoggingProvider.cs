using Serilog;
using Serilog.Core;

namespace RainBot.Core
{
    public static class LoggingProvider
    {
        public static Logger GetLogger()
        {
            //return GetConsoleLogger();
            return GetSqlLogger();
        }

        private static Logger GetConsoleLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static Logger GetSqlLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.MSSqlServer(BotConfig.GetValue("ConnectionString"), "Log", Serilog.Events.LogEventLevel.Error, autoCreateSqlTable: true)
                .CreateLogger();
        }
    }
}