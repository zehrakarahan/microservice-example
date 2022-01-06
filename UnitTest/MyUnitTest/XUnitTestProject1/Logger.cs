using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace XUnitTestProject1
{
    public class Logger
    {
        private static ILoggerFactory _loggerFactory;

        public static void SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public static void Error(string message)
        {
            var logger = CreateLogger();
            logger.LogError(message);
        }

        public static void Error(Exception ex, string message)
        {
            var logger = CreateLogger();
            logger.LogError(ex, message);
        }

        private static ILogger CreateLogger()
        {
            if (_loggerFactory == null)
            {
                throw new ArgumentNullException($"No LoggerFactory initialized, please invoke {nameof(SetLoggerFactory)}.");
            }
            return _loggerFactory.CreateLogger((new StackTrace()).GetFrame(2).GetMethod().ReflectedType);
        }
    }
}
