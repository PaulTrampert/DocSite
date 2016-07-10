using System.IO;
using DocSite.Renderers;
using DocSite.SiteModel;
using DocSite.TemplateLoaders;
using DocSite.Xml;

namespace DocSite
{
    /// <summary>
    /// Entry point class of DocSite.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Some Generic method.
        /// </summary>
        /// <typeparam name="T">Type of the argument.</typeparam>
        /// <param name="arg">The argument</param>
        public static void GenericMethod<T>(T arg)
        {
            
        }

        /// <summary>
        /// Entry point of DocSite
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            var xmlPath = Path.GetFullPath(args[0]);
            var outDir = Path.GetFullPath(args[1]);
            var builder = new ModelBuilder();
            var xmlModel = builder.BuildModelFromXml(xmlPath);
            var docModel = new DocSiteModel(xmlModel);
            var htmlTemplateLoader = new EmbeddedTemplateLoader("DocSite.Templates.Html");
            var cssTemplateLoader = new EmbeddedTemplateLoader("DocSite.Templates.Html.css");
            var scriptsTemplateLoader = new EmbeddedTemplateLoader("DocSite.Templates.Html.scripts");
            var renderer = new HtmlRenderer(htmlTemplateLoader, cssTemplateLoader, scriptsTemplateLoader, docModel);
            renderer.RenderSite(docModel, outDir);
        }
    }
}
