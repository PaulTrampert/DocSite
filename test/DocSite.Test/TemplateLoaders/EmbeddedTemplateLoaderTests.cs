using DocSite.TemplateLoaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DocSite.Test.TemplateLoaders
{
    public class EmbeddedTemplateLoaderTests
    {
        [Theory]
        [InlineData("DocSite.Templates.Html", "c.html")]
        [InlineData("DocSite.Templates.Html", "code.html")]
        [InlineData("DocSite.Templates.Html", "Page.html")]
        [InlineData("DocSite.Templates.Html", "para.html")]
        [InlineData("DocSite.Templates.Html", "Section.html")]
        [InlineData("DocSite.Templates.Html", "DefinitionsSection.html")]
        [InlineData("DocSite.Templates.Html", "TableSection.html")]
        [InlineData("DocSite.Templates.Html", "TableRow.html")]
        public void CanLoadTemplate(string templateNamespace, string templateName)
        {
            var loader = new EmbeddedTemplateLoader(templateNamespace);
            string result = loader.LoadTemplate(templateName);
            Assert.NotNull(result);
        }
    }
}
