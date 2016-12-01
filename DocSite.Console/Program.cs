using DocSite;
using DocSite.Renderers;
using DocSite.SiteModel;
using DocSite.TemplateLoaders;
using DocSite.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Reflection;
using System.IO;

namespace DocSite.Console
{
    /// <summary>
    /// Entry point class of DocSite.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point of DocSite
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static int Main(string[] args)
        {
            var arguments = new Arguments();
            var app = new CommandLineApplication();
            var inputFileArg = app.Argument("InputFile", "The documentation xml generated from your code.");
            var rendererOpt = app.Option("-r|--renderer", "The renderer to use. Defaults to Html.", CommandOptionType.SingleValue,
                opt => 
                Enum.Parse(typeof(RendererOptions), opt.Value() ?? "Html"));
            var outputOpt = app.Option("-o|--output-directory", "Directory to render to. Defaults to the current directory.", CommandOptionType.SingleValue);
            var logLevelOpt = app.Option("-l|--log-level", "The minimum logging level to output. Defaults to Information.", CommandOptionType.SingleValue);
            app.HelpOption("-h|--help|-help|-?");

            app.OnExecute(() => {
                arguments.DocXml = inputFileArg.Value;
                arguments.Renderer = (RendererOptions)Enum.Parse(typeof(RendererOptions), rendererOpt.Value() ?? "Html", true);
                arguments.OutputDirectory = outputOpt.Value() ?? Directory.GetCurrentDirectory();
                arguments.LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logLevelOpt.Value() ?? "Information", true);
                var logFactory = new LoggerFactory().AddConsole(arguments.LogLevel);
                var logger = logFactory.CreateLogger<Program>();
                try
                {
                    SiteBuilder.BuildSite(arguments, logFactory);
                }
                catch (Exception e)
                {
                    logger.LogError(default(EventId), e, e.Message);
                    return 2;
                }
                return 0;
            });
            return app.Execute(args);
        }

        
    }
}