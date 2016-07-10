using DocSite.Pages;
using System;
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
    public class HtmlRenderer : IRenderer
    {
        private ITemplateLoader _templateLoader;
        private DocSiteModel _context;

        public HtmlRenderer(ITemplateLoader templateLoader, DocSiteModel context)
        {
            _templateLoader = templateLoader;
            _context = context;
        }

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
                        c =>
                            c.Link == null
                                ? RenderNode(c.Content)
                                : $"<a href=\"{c.Link}.html\">{RenderNode(c.Content)}</a>"));
                rows.Append(rowTemplate.Replace("@Columns", columns));
            }
            return tableTemplate
                .Replace("@Title", section.Title)
                .Replace("@Headers", headers)
                .Replace("@Rows", rows.ToString());
        }

        public string RenderNodes(IEnumerable<XmlNode> nodes)
        {
            var result = new StringBuilder();
            foreach (var node in nodes)
            {
                result.Append(RenderNode(node));
            }
            return result.ToString();
        }

        public string RenderNode(XmlNode node)
        {
            var templateName = $"{node.Name.Trim('#')}.html";
            var template = _templateLoader.LoadTemplate(templateName);
            if (template == null) template = "@Body";

            var body = new StringBuilder();
            if (node.HasChildNodes)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    body.Append(RenderNode(node.ChildNodes[i]));
                }
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
                    template = template.Replace("@Cref", "#");
                    template = template.Replace("@CrefText", memberDetails.FullName);
                }
                else
                {
                    memberDetails = refMember.MemberDetails;
                    template = template.Replace("@Cref", $"{memberDetails.FileId}.html");
                    template = template.Replace("@CrefText", memberDetails.LocalName);
                }
            }
            if (node.Attributes?["name"] != null)
            {
                template = template.Replace("@Name", node.Attributes["name"].Value);
            }
            return template.Replace("@Body", body.ToString());
        }

        public string RenderDefinitionsSection(DefinitionsSection section)
        {
            throw new NotImplementedException();
        }
    }

    internal class TemplateNotFoundException : Exception
    {
        public TemplateNotFoundException()
        {
        }

        public TemplateNotFoundException(string templateName) : base($"{templateName} could not be found")
        {
        }
    }
}
