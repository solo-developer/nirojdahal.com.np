using Serilog;
using Serilog.Formatting.Json;
using ILogger = Serilog.ILogger;

namespace niroj.website.Logging
{
    public class LogSerilog : ILog
    {
        private static readonly ILogger _logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.RollingFile(new JsonFormatter(), "Logs/ErrorLog-{Date}.json", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
               //.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();


        public LogSerilog()
        {
        }

        public void Information(string message)
        {
            _logger.Information(message);
        }

        public void Warning(string message)
        {
            _logger.Warning(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }
    }
}
