using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace DocSite.Targets
{
    public class MsBuildLogger : Microsoft.Extensions.Logging.ILogger
    {
        private TaskLoggingHelper logger;
        private string category;
        private IBuildEngine buildEngine;

        public MsBuildLogger(TaskLoggingHelper logger, IBuildEngine buildEngine, string category)
        {
            this.logger = logger;
            this.buildEngine = buildEngine;
            this.category = category;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new Scope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            switch(logLevel)
            {
                case LogLevel.Critical:
                    logger.LogCriticalMessage(category, eventId.Name, null, buildEngine.ProjectFileOfTaskNode, buildEngine.LineNumberOfTaskNode, buildEngine.ColumnNumberOfTaskNode, 0, 0, formatter(state, exception));
                    break;
                case LogLevel.Error:
                    logger.LogError(category, eventId.Name, null, buildEngine.ProjectFileOfTaskNode, buildEngine.LineNumberOfTaskNode, buildEngine.ColumnNumberOfTaskNode, 0, 0, formatter(state, exception));
                    break;
                case LogLevel.Warning:
                    logger.LogWarning(category, eventId.Name, null, buildEngine.ProjectFileOfTaskNode, buildEngine.LineNumberOfTaskNode, buildEngine.ColumnNumberOfTaskNode, 0, 0, formatter(state, exception));
                    break;
                case LogLevel.Debug:
                    logger.LogMessage(category, eventId.Name, null, buildEngine.ProjectFileOfTaskNode, buildEngine.LineNumberOfTaskNode, buildEngine.ColumnNumberOfTaskNode, 0, 0, MessageImportance.Normal, formatter(state, exception));
                    break;
                case LogLevel.Trace:
                    logger.LogMessage(category, eventId.Name, null, buildEngine.ProjectFileOfTaskNode, buildEngine.LineNumberOfTaskNode, buildEngine.ColumnNumberOfTaskNode, 0, 0, MessageImportance.Low, formatter(state, exception));
                    break;
                default:
                    logger.LogMessage(category, eventId.Name, null, buildEngine.ProjectFileOfTaskNode, buildEngine.LineNumberOfTaskNode, buildEngine.ColumnNumberOfTaskNode, 0, 0, MessageImportance.High, formatter(state, exception));
                    break;
            }
        }

        private class Scope : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
