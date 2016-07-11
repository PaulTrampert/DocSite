﻿using System;
using System.IO;
using System.Reflection;
using DocSite.Renderers;
using DocSite.SiteModel;
using DocSite.TemplateLoaders;
using DocSite.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using PTrampert.AppArgs;

namespace DocSite
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
        public static void Main(string[] args)
        {
            var argParser = new MixedParser<Arguments>();
            var arguments = argParser.Parse(args);
            if (arguments.Help)
            {
                var helpBuilder = new HelpBuilder<Arguments>();
                Console.WriteLine(
                    helpBuilder.BuildHelp($"dotnet {typeof(Program).GetTypeInfo().Assembly.GetName().Name}.dll"));
                return;
            }
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
            }
        }
    }
}