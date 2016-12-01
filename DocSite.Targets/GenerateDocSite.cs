using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;

namespace DocSite.Targets
{
    public class GenerateDocSite : Task
    {
        [Required]
        public string DocXml { get; set; }

        public RendererOptions Renderer { get; set; }

        public string OutputDirectory { get; set; }

        public override bool Execute()
        {
            var arguments = new Arguments
            {
                DocXml = DocXml,
                Renderer = Renderer,
                OutputDirectory = OutputDirectory
            };

            var loggingFactory = new LoggerFactory();
            loggingFactory.AddProvider(new MsBuildLoggerProvider(Log, BuildEngine));

            try
            {
                SiteBuilder.BuildSite(arguments, loggingFactory);
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true, true, this.BuildEngine.ProjectFileOfTaskNode);
            }
            return true;
        }

        
    }
}
