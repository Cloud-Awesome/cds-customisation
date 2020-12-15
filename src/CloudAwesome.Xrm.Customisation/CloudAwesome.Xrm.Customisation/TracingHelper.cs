using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Customisation
{
    public class TracingHelper
    {
        private readonly ILogger _logger;

        public TracingHelper(ILogger logger)
        {
            if (logger != null)
            {
                _logger = logger;
            }
        }

        public void Log(LogLevel logLevel, string message)
        {
            _logger?.Log(logLevel, message);
        }

        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Log(LogLevel.Information, message);
        }

        public void Critical(string message)
        {
            Log(LogLevel.Critical, message);
        }

        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        public void Trace(string message)
        {
            Log(LogLevel.Trace, message);
        }

        public void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }
    }
}
