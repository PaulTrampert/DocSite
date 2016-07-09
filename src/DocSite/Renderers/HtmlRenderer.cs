using DocSite.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DocSite.TemplateLoaders;

namespace DocSite.Renderers
{
    public class HtmlRenderer : IRenderer
    {
        private ITemplateLoader _templateLoader;

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
                body.Append(RenderSection(section));
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

        public string RenderNode(XmlNode node)
        {
            var templateName = $"{node.Name.Trim('#')}.html";
            var template = _templateLoader.LoadTemplate(templateName);
            if (template == null) throw new TemplateNotFoundException(templateName);

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
                template = template.Replace("@Cref", $"{node.Attributes["cref"].Value}.html");
            }
            return template.Replace("@Body", body.ToString());
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
