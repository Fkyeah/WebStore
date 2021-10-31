﻿using log4net;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(string category, XmlElement configuration)
        {
            var logger_repository = LogManager
               .CreateRepository(
                    Assembly.GetEntryAssembly(),
                    typeof(log4net.Repository.Hierarchy.Hierarchy));

            _log = LogManager.GetLogger(logger_repository.Name, category);

            log4net.Config.XmlConfigurator.Configure(configuration);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel level) =>
            level switch
            {
                LogLevel.None => false,
                LogLevel.Trace => _log.IsDebugEnabled,
                LogLevel.Debug => _log.IsDebugEnabled,
                LogLevel.Information => _log.IsInfoEnabled,
                LogLevel.Warning => _log.IsWarnEnabled,
                LogLevel.Error => _log.IsErrorEnabled,
                LogLevel.Critical => _log.IsFatalEnabled,
                _ => throw new InvalidEnumArgumentException(nameof(level), (int)level, typeof(LogLevel))
            };

        public void Log<TState>(LogLevel level, EventId id, TState state, Exception error, Func<TState, Exception, string> formatter)
        {
            if (formatter is null)
                throw new ArgumentNullException(nameof(formatter));

            if (!IsEnabled(level))
                return;

            var log_string = formatter(state, error);
            if (string.IsNullOrEmpty(log_string) && error is null)
                return;

            switch (level)
            {
                default:
                    throw new InvalidEnumArgumentException(nameof(level), (int)level, typeof(LogLevel));

                case LogLevel.None:
                    break;

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _log.Debug(log_string);
                    break;

                case LogLevel.Information:
                    _log.Info(log_string);
                    break;

                case LogLevel.Warning:
                    _log.Warn(log_string);
                    break;

                case LogLevel.Error:
                    _log.Error(log_string, error);
                    break;

                case LogLevel.Critical:
                    _log.Fatal(log_string, error);
                    break;
            }
        }
    }
}