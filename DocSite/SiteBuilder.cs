using DocSite.Renderers;
using DocSite.SiteModel;
using DocSite.TemplateLoaders;
using DocSite.Xml;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocSite
{
    /// <summary>
    /// Class that processes the arguments and build the documentation site.
    /// </summary>
    public class SiteBuilder
    {
        /// <summary>
        /// Process the arguments and build the documentation site.
        /// </summary>
        /// <param name="arguments">Arguments for building the site.</param>
        /// <param name="logFactory">Log factory for obtaining the appropriate logger.</param>
        public static void BuildSite(Arguments arguments, ILoggerFactory logFactory)
        {
            var logger = logFactory.CreateLogger<SiteBuilder>();
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
    }
}
