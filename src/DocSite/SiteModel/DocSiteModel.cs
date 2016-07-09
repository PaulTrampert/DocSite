using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public class DocSiteModel
    {
        public string AssemblyName { get; }

        public IEnumerable<DocNamespace> Namespaces { get; }

        public IDictionary<string, IDocModel> MembersDictionary { get; }

        public DocSiteModel(DocXmlModel xmlModel)
        {
            AssemblyName = xmlModel.Assembly.Name;
            var namespaces = new List<string>();
            var typesByParent = xmlModel.Members.Where(m => m.Type == MemberType.Type).GroupBy(m => m.ParentMember);
            foreach (var parentTypeMapping in typesByParent)
            {
                if (!xmlModel.Members.Any(m => m.FullName == parentTypeMapping.Key))
                    namespaces.Add(parentTypeMapping.Key);
            }
            Namespaces = namespaces.Distinct().Select(n => new DocNamespace(new MemberDetails {Id = $"N:{n}"}, xmlModel.Members));

            MembersDictionary = new Dictionary<string, IDocModel>();
            foreach (var ns in Namespaces)
            {
                ns.AddMembersToDictionary(MembersDictionary);
            }
        }
    }
}
