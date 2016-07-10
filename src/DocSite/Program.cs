using System;
using System.IO;
using DocSite.Renderers;
using DocSite.SiteModel;
using DocSite.TemplateLoaders;
using DocSite.Xml;
using Microsoft.Extensions.Logging;

namespace DocSite
{
    /// <summary>
    /// Entry point class of DocSite.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Logger factory to be used throughout the program.
        /// </summary>
        public static readonly ILoggerFactory LoggerFactory = new LoggerFactory().AddConsole();

        /// <summary>
        /// Entry point of DocSite
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            var logger = LoggerFactory.CreateLogger<Program>();
            try
            {
                var xmlPath = Path.GetFullPath(args[0]);
                var outDir = Path.GetFullPath(args[1]);
                var builder = new ModelBuilder();
                var xmlModel = builder.BuildModelFromXml(xmlPath);
                var docModel = new DocSiteModel(xmlModel);
                logger.LogInformation($"Documentation model built from {xmlPath}");
                var htmlTemplateLoader = new EmbeddedTemplateLoader("DocSite.Templates.Html");
                var cssTemplateLoader = new EmbeddedTemplateLoader("DocSite.Templates.Html.css");
                var scriptsTemplateLoader = new EmbeddedTemplateLoader("DocSite.Templates.Html.scripts");
                var renderer = new HtmlRenderer(htmlTemplateLoader, cssTemplateLoader, scriptsTemplateLoader, docModel);
                renderer.RenderSite(docModel, outDir);
                logger.LogInformation($"Site rendered to {outDir}");
            }
            catch (Exception e)
            {
                logger.LogError(default(EventId), e, e.Message);
            }
        }
    }
}
