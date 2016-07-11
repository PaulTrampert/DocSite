using DocSite.Pages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DocSite.SiteModel;
using DocSite.TemplateLoaders;
using DocSite.Xml;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DocSite.Renderers
{
    /// <summary>
    /// Renderer that renders to HTML.
    /// </summary>
    /// <seealso cref="IRenderer"/>
    public class HtmlRenderer : IRenderer
    {
        private readonly ITemplateLoader _htmlTemplateLoader;
        private readonly ITemplateLoader _cssTemplateLoader;
        private readonly ITemplateLoader _scriptsTemplateLoader;
        private DocSiteModel _context;
        private JsonSerializerSettings _jsonSettings;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new <see cref="HtmlRenderer"/>.
        /// </summary>
        /// <param name="htmlTemplateLoader">The <see cref="ITemplateLoader"/> to use to find templates.</param>
        /// <param name="context">The <see cref="DocSiteModel"/> that will be rendered.</param>
        /// <param name="scriptsTemplateLoader"></param>
        /// <param name="cssTemplateLoader"></param>
        public HtmlRenderer(ITemplateLoader htmlTemplateLoader, ITemplateLoader cssTemplateLoader, ITemplateLoader scriptsTemplateLoader, DocSiteModel context, ILoggerFactory logFactory = null)
        {
            _logger = (logFactory ?? new LoggerFactory().AddConsole()).CreateLogger<HtmlRenderer>();
            _htmlTemplateLoader = htmlTemplateLoader;
            _cssTemplateLoader = cssTemplateLoader;
            _scriptsTemplateLoader = scriptsTemplateLoader;
            _context = context;

            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            };
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>
        /// </summary>
        /// <param name="site"></param>
        /// <param name="outDir"></param>
        public void RenderSite(DocSiteModel site, string outDir)
        {
            var outPath = Path.GetFullPath(outDir);
            Directory.CreateDirectory(outPath);
            var scriptsPath = Path.Combine(outPath, "scripts");
            Directory.CreateDirectory(scriptsPath);
            var cssPath = Path.Combine(outPath, "css");
            Directory.CreateDirectory(cssPath);

            _logger.LogInformation($"Rendering html to {outPath}");
            _logger.LogInformation($"Rendering scripts to {scriptsPath}");
            _logger.LogInformation($"Rendering css to {cssPath}");

            foreach (var script in _scriptsTemplateLoader.LoadAllTemplates())
            {
                File.WriteAllText(Path.Combine(scriptsPath, script.Key), script.Value);
                _logger.LogInformation($"Rendered script: {script.Key}");
            }

            foreach (var css in _cssTemplateLoader.LoadAllTemplates())
            {
                File.WriteAllText(Path.Combine(cssPath, css.Key), css.Value);
                _logger.LogInformation($"Rendered css: {css.Key}");
            }


            var pages = site.BuildPages();
            var pageCount = pages.Count();
            var i = 1;
            _logger.LogInformation($"{pageCount} pages will be rendered.");
            foreach (var page in pages)
            {
                var tree = new [] {site.BuildTree(page.Name, "html")};
                using (var writer = new StreamWriter(File.Create(Path.Combine(outPath, $"{page.Name}.html"))))
                {
                    RenderPageTo(page, tree, writer);
                    _logger.LogInformation($"Rendered page {i}/{pageCount}");
                }
                i++;
            }
        }

        private void RenderPageTo(Page page, IEnumerable<Tree> tree, TextWriter writer)
        {
            var render = page.RenderWith(this);
            render = render.Replace("@NavJson", JsonConvert.SerializeObject(tree, _jsonSettings));
            writer.Write(render);
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderPage(Page page)
        {
            var templateName = "Page.html";
            var template = _htmlTemplateLoader.LoadTemplate(templateName);
            if (template == null) throw new TemplateNotFoundException(templateName);

            template = template.Replace("@AssemblyName", page.AssemblyName);
            template = template.Replace("@Title", page.Title);
            var body = new StringBuilder();
            foreach (var section in page.Sections)
            {
                body.Append(section.RenderWith(this));
            }
            return template.Replace("@Body", body.ToString());
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderSection(Section section)
        {
            var templateName = "Section.html";
            var template = _htmlTemplateLoader.LoadTemplate(templateName);
            if (template == null) throw new TemplateNotFoundException(templateName);

            var body = new StringBuilder();
            foreach (var node in section.Body)
            {
                body.Append(RenderNode(node));
            }
            template = template.Replace("@Title", section.Title);
            return template.Replace("@Body", body.ToString());
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderTableSection(TableSection section)
        {
            var templateName = "TableSection.html";
            var tableTemplate = _htmlTemplateLoader.LoadTemplate(templateName);
            if (tableTemplate == null) throw new TemplateNotFoundException(templateName);

            templateName = "TableRow.html";
            var rowTemplate = _htmlTemplateLoader.LoadTemplate(templateName);
            if (rowTemplate == null) throw new TemplateNotFoundException(templateName);

            var headers = string.Join("", section.Headers.Select(h => $"<th>{h}</th>"));
            var rows = new StringBuilder();
            foreach (var row in section.Rows)
            {
                var columns = string.Join("",
                    row.Columns.Select(
                        c =>$"<td>{(c.Link == null ? RenderTableData(c) : $"<a href=\"{c.Link}.html\">{RenderTableData(c)}</a>")}</td>"));
                rows.Append(rowTemplate.Replace("@Columns", columns));
            }
            return tableTemplate
                .Replace("@Title", section.Title)
                .Replace("@Headers", headers)
                .Replace("@Rows", rows.ToString());
        }

        private string RenderTableData(TableData data)
        {
            return data.XmlContent == null ? data.TextContent ?? "" : RenderNode(data.XmlContent);
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderNodes(IEnumerable<XmlNode> nodes)
        {
            return string.Join(" ", nodes.Select(n => RenderNode(n)));
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderNode(XmlNode node)
        {
            var templateName = $"{node.Name.Trim('#')}.html";
            var template = _htmlTemplateLoader.LoadTemplate(templateName);
            if (template == null) template = "@Body";

            var body = new StringBuilder();
            if (node.HasChildNodes)
            {
                body.Append(RenderNodes(node.ChildNodes.Cast<XmlNode>()));
            }
            else
            {
                body.Append(node.InnerText.Trim());
            }
            if (node.Attributes?["cref"] != null)
            {
                IDocModel refMember = null;
                MemberDetails memberDetails = null;
                var isProjectReference = _context.MembersDictionary.TryGetValue(node.Attributes["cref"].Value, out refMember);
                if (!isProjectReference)
                {
                    memberDetails = new MemberDetails {Id = node.Attributes["cref"].Value};
                    template = template.Replace("@CrefText", memberDetails.FullName);
                    template = template.Replace("@Cref", "#");
                }
                else
                {
                    memberDetails = refMember.MemberDetails;
                    template = template.Replace("@CrefText", memberDetails.LocalName);
                    template = template.Replace("@Cref", $"{memberDetails.FileId}.html");
                }
            }
            if (node.Attributes?["name"] != null)
            {
                template = template.Replace("@Name", node.Attributes["name"].Value);
            }
            return template.Replace("@Body", body.ToString());
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderDefinitionsSection(DefinitionsSection section)
        {
            var templateName = "DefinitionsSection.html";
            var template = _htmlTemplateLoader.LoadTemplate(templateName);
            if (template == null) throw new TemplateNotFoundException(templateName);

            var body = RenderNodes(section.Definitions);
            template = template.Replace("@Title", section.Title);
            return template.Replace("@Body", body.ToString());
        }
    }
}
