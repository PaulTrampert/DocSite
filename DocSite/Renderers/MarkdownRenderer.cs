using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DocSite.Pages;
using DocSite.SiteModel;
using DocSite.TemplateLoaders;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.IO;
using DocSite.Xml;

namespace DocSite.Renderers
{
    /// <summary>
    /// Renders a single markdown page containing the documentation.
    /// </summary>
    public class MarkdownRenderer : IRenderer
    {
        private const string SectionTemplate = "Section.md";
        private const string PageTemplate = "Page.md";
        private const string DefinitionsSectionTemplate = "DefinitionsSection.md";
        ITemplateLoader templateLoader;
        ILogger logger;
        private DocSiteModel context;

        /// <summary>
        /// Constructor for MarkdownRenderer
        /// </summary>
        /// <param name="templateLoader">Template loader for markdown templates.</param>
        /// <param name="loggerFactory">Logger factory for providing a MarkdownRenderer logger.</param>
        public MarkdownRenderer(ITemplateLoader templateLoader, ILoggerFactory loggerFactory)
        {
            this.templateLoader = templateLoader;
            this.logger = loggerFactory.CreateLogger<MarkdownRenderer>();
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderDefinitionsSection(DefinitionsSection section)
        {
            logger.LogTrace($"Rendering {typeof(DefinitionsSection)}: {section.Title}");
            var template = templateLoader.LoadTemplate(DefinitionsSectionTemplate);
            if (template == null) throw new TemplateNotFoundException(DefinitionsSectionTemplate);

            var body = RenderNodes(section.Definitions);
            template = template.Replace("@Title", section.Title);
            return template.Replace("@Body", body.ToString());
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderNode(XmlNode node)
        {
            logger.LogTrace($"Rendering {node.Name} node");
            var templateName = $"{node.Name.Trim('#')}.md";
            var template = templateLoader.LoadTemplate(templateName);
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
                var isProjectReference = context.MembersDictionary.TryGetValue(node.Attributes["cref"].Value, out refMember);
                if (!isProjectReference)
                {
                    memberDetails = new MemberDetails { Id = node.Attributes["cref"].Value };
                    template = template.Replace("@CrefText", memberDetails.FullName);
                    template = template.Replace("@Cref", "#");
                }
                else
                {
                    memberDetails = refMember.MemberDetails;
                    template = template.Replace("@CrefText", memberDetails.LocalName);
                    template = template.Replace("@Cref", $"{memberDetails.LocalName}");
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
        public string RenderNodes(IEnumerable<XmlNode> nodes)
        {
            logger.LogTrace($"Rendering {nodes.Count()} nodes");
            return string.Join(" ", nodes.Select(n => RenderNode(n)));
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderPage(Page page)
        {
            logger.LogTrace($"Rendering {typeof(Page).Name}: {page.Title}");
            var template = templateLoader.LoadTemplate(PageTemplate);
            if (template == null) throw new TemplateNotFoundException(PageTemplate);
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
            logger.LogTrace($"Rendering {typeof(Section).Name}: {section.Title}");
            var template = templateLoader.LoadTemplate(SectionTemplate);
            if (template == null) throw new TemplateNotFoundException(SectionTemplate);
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
        public void RenderSite(DocSiteModel site, string outDir)
        {
            context = site;
            var pages = site.BuildPages();
            var pageCount = pages.Count();
            var i = 1;
            logger.LogInformation($"{pageCount} pages will be rendered.");
            using (var writer = new StreamWriter(File.Create(Path.Combine(outDir, $"{site.AssemblyName}.md"))))
            {
                foreach (var page in pages)
                {
                    logger.LogTrace($"Rendering page {page.Name}");
                    writer.Write(page.RenderWith(this));
                    logger.LogDebug($"Rendered page {i}/{pageCount}");
                    i++;
                }
            }
            context = null;
        }

        /// <summary>
        /// Inherited from <see cref="IRenderer"/>.
        /// </summary>
        public string RenderTableSection(TableSection section)
        {
            logger.LogTrace($"Rendering {typeof(TableSection).Name}: {section.Title}");
            var templateName = "TableSection.md";
            var tableTemplate = templateLoader.LoadTemplate(templateName);
            if (tableTemplate == null) throw new TemplateNotFoundException(templateName);

            templateName = "TableRow.md";
            var rowTemplate = templateLoader.LoadTemplate(templateName);
            if (rowTemplate == null) throw new TemplateNotFoundException(templateName);

            var headers = string.Join("", section.Headers.Select(h => $"| {h} "));
            headers += "|\r\n" + string.Join("", section.Headers.Select(h => "| --- "));
            var rows = new StringBuilder();
            foreach (var row in section.Rows)
            {
                var columns = string.Join("",
                    row.Columns.Select(
                        c => $"| {(c.Link == null ? RenderTableData(c) : $"[{RenderTableData(c)}](#{c.Link})")} "));
                rows.Append(rowTemplate.Replace("@Columns", columns));
            }
            return tableTemplate
                .Replace("@Title", section.Title)
                .Replace("@Headers", headers)
                .Replace("@Rows", rows.ToString());
        }

        private string RenderTableData(TableData data)
        {
            logger.LogTrace($"Rendering {typeof(TableData).Name}");
            return data.XmlContent == null ? data.TextContent ?? "" : RenderNode(data.XmlContent);
        }
    }
}
