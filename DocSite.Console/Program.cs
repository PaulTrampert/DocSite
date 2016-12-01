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
            app.Argument("Input File", "The documentation xml generated from your code.", 
                arg => arguments.DocXml = arg.Value);
            app.Option("-r|--renderer", "The renderer to use. Defaults to Html.", CommandOptionType.SingleValue,
                opt => Enum.Parse(typeof(RendererOptions), opt.Value() ?? "Html"));
            app.Option("-o|--output-directory", "Directory to render to. Defaults to the current directory.", CommandOptionType.SingleValue,
                opt => arguments.OutputDirectory = opt.Value() ?? Directory.GetCurrentDirectory());
            app.Option("-l|--log-level", "The minimum logging level to output. Defaults to Information.", CommandOptionType.SingleValue,
                opt => Enum.Parse(typeof(LogLevel), opt.Value() ?? "Information"));
            app.HelpOption("-h|--help|-help|-?");

            app.OnExecute(() => { 
                var logFactory = new LoggerFactory().AddConsole(arguments.LogLevel);
                var logger = logFactory.CreateLogger<Program>();

                try
                {
                    var builder = new ModelBuilder();
                    var xmlModel = builder.BuildModelFromXml(arguments.DocXml);
                    var docModel = new DocSiteModel(xmlModel);
                    logger.LogInformation($"Documentation model built from {arguments.DocXml}");
                    IRenderer renderer = null;
                    switch (arguments.Renderer)
                    {
                        case RendererOptions.Html:
                        default:
                            var htmlTemplateLoader = new EmbeddedTemplateLoader("DocSite.Templates.Html");
                            var cssTemplateLoader = new EmbeddedTemplateLoader("DocSite.Templates.Html.css");
                            var scriptsTemplateLoader = new EmbeddedTemplateLoader("DocSite.Templates.Html.scripts");
                            renderer = new HtmlRenderer(htmlTemplateLoader, cssTemplateLoader, scriptsTemplateLoader,
                                docModel, logFactory);
                            logger.LogInformation($"Using Renderer: {typeof(HtmlRenderer).Name}");
                            break;
                    }
                    renderer.RenderSite(docModel, arguments.OutputDirectory);
                    logger.LogInformation($"Site rendered to {arguments.OutputDirectory}");
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