using System.IO;
using DocSite.Renderers;
using DocSite.SiteModel;
using DocSite.TemplateLoaders;
using DocSite.Xml;

namespace DocSite
{
    public class Program
    {
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
            var renderer = new HtmlRenderer(htmlTemplateLoader, docModel);
            Directory.CreateDirectory(outDir);
            foreach (var page in docModel.BuildPages())
            {
                var outFile = Path.Combine(outDir, $"{page.Name}.html");
                File.WriteAllText(outFile, page.RenderWith(renderer));
            }
        }
    }
}
