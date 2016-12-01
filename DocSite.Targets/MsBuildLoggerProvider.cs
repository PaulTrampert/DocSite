using Microsoft.Build.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;

namespace DocSite.Targets
{
    public class MsBuildLoggerProvider : ILoggerProvider
    {
        private IBuildEngine buildEngine;
        private TaskLoggingHelper logger;

        public MsBuildLoggerProvider(TaskLoggingHelper logger)
        {
            this.logger = logger;
        }

        public MsBuildLoggerProvider(TaskLoggingHelper logger, IBuildEngine buildEngine) : this(logger)
        {
            this.buildEngine = buildEngine;
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new MsBuildLogger(logger, buildEngine, categoryName);
        }

        public void Dispose()
        {
        }
    }
}
