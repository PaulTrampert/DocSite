using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace DocSite.TemplateLoaders
{
    /// <summary>
    /// Template loader that loads templates from embedded resources.
    /// </summary>
    public class EmbeddedTemplateLoader : ITemplateLoader
    {
        private Assembly _assembly;

        private string _templateNamespace;

        /// <summary>
        /// Create a new <see cref="EmbeddedTemplateLoader"/>
        /// </summary>
        /// <param name="templateNamespace">The namespace to find tempaltes in.</param>
        public EmbeddedTemplateLoader(string templateNamespace)
        {
            _assembly = typeof(EmbeddedTemplateLoader).GetTypeInfo().Assembly;
            _templateNamespace = templateNamespace;
        }

        /// <summary>
        /// Inherited from <see cref="ITemplateLoader"/>
        /// </summary>
        public string LoadTemplate(string name)
        {
            try
            {
                using (
                    var reader = new StreamReader(_assembly.GetManifestResourceStream($"{_templateNamespace}.{name}")))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Inherited from <see cref="ITemplateLoader"/>
        /// </summary>
        public IDictionary<string, string> LoadAllTemplates()
        {
            var templateNames = _assembly.GetManifestResourceNames();
            var result = new Dictionary<string, string>();
            foreach (var name in templateNames.Where(tn => tn.StartsWith(_templateNamespace)))
            {
                var scopedName = name.Replace($"{_templateNamespace}.", "");
                using (var reader = new StreamReader(_assembly.GetManifestResourceStream(name)))
                {
                    result.Add(scopedName, reader.ReadToEnd());
                }
            }
            return result;
        }
    }
}
