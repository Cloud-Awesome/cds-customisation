﻿using System;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Customisation.Tests.Stubs
{
    public class StubLogger: ILogger
    {
        #pragma warning disable 649
        private readonly LogLevel _logLevel;
        #pragma warning restore 649

        public string ResponseMessage;
        public LogLevel ResponseLogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            ResponseLogLevel = logLevel;
            ResponseMessage = state.ToString();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}