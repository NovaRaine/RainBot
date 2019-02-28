using System.Collections.Generic;
using NpgsqlTypes;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;

namespace RainBot.Core
{
    public static class LoggingProvider
    {
        public static Logger GetLogger()
        {
#if DEBUG
            return GetConsoleLogger();
#endif
            return GetPostgreSqlLogger();
        }

        private static Logger GetConsoleLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static Logger GetPostgreSqlLogger()
        {
            string tableName = "logs";

            //Used columns (Key is a column name)
            //Column type is writer's constructor parameter
            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
            };

            return new LoggerConfiguration()
                .WriteTo.PostgreSQL(BotConfig.GetValue("ConnectionString"), tableName, columnWriters, needAutoCreateTable: true)
                .CreateLogger();
        }
    }
}