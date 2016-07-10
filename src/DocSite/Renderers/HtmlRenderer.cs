using DocSite.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DocSite.SiteModel;
using DocSite.TemplateLoaders;
using DocSite.Xml;

namespace DocSite.Renderers
{
    /// <summary>
    /// Renderer that renders to HTML.
    /// </summary>
    /// <seealso cref="IRenderer"/>
    public class HtmlRenderer : IRenderer
    {
        private ITemplateLoader _templateLoader;
        private DocSiteModel _context;

        /// <summary>
        /// Creates a new <see cref="HtmlRenderer"/>.
        /// </summary>
        /// <param name="templateLoader">The <see cref="ITemplateLoader"/> to use to find templates.</param>
        /// <param name="context">The <see cref="DocSiteModel"/> that will be rendered.</param>
        public HtmlRenderer(ITemplateLoader templateLoader, DocSiteModel context)
        {
            _templateLoader = templateLoader;
            _context = context;
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderPage(Page page)
        {
            var templateName = "Page.html";
            var template = _templateLoader.LoadTemplate(templateName);
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
            var template = _templateLoader.LoadTemplate(templateName);
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
            var tableTemplate = _templateLoader.LoadTemplate(templateName);
            if (tableTemplate == null) throw new TemplateNotFoundException(templateName);

            templateName = "TableRow.html";
            var rowTemplate = _templateLoader.LoadTemplate(templateName);
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
            var template = _templateLoader.LoadTemplate(templateName);
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
            var template = _templateLoader.LoadTemplate(templateName);
            if (template == null) throw new TemplateNotFoundException(templateName);

            var body = RenderNodes(section.Definitions);
            template = template.Replace("@Title", section.Title);
            return template.Replace("@Body", body.ToString());
        }
    }
}
