using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string _configurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new();

        public Log4NetLoggerProvider(string configurationFile) => _configurationFile = configurationFile;

        public ILogger CreateLogger(string Category) =>
            _loggers.GetOrAdd(Category, category =>
            {
                var xml = new XmlDocument();
                xml.Load(_configurationFile);
                return new Log4NetLogger(category, xml["log4net"]);
            });

        public void Dispose() => _loggers.Clear();
    }
}
