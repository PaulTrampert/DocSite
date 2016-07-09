using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace DocSite.TemplateLoaders
{
    public class EmbeddedTemplateLoader : ITemplateLoader
    {
        private Assembly _assembly;

        private string _templateNamespace;

        public EmbeddedTemplateLoader(string templateNamespace)
        {
            _assembly = typeof(EmbeddedTemplateLoader).GetTypeInfo().Assembly;
            _templateNamespace = templateNamespace;
        }
        public string LoadTemplate(string name)
        {
            var templateNames = _assembly.GetManifestResourceNames();
            using (var reader = new StreamReader(_assembly.GetManifestResourceStream($"{_templateNamespace}.{name}")))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
