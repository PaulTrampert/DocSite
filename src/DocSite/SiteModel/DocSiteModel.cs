using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public class DocSiteModel
    {
        public string AssemblyName { get; set; }

        public IEnumerable<DocType> Types { get; set; }

        public DocSiteModel(DocXmlModel xmlModel)
        {
            AssemblyName = xmlModel.Assembly.Name;
            Types = xmlModel.Members.Where(m => m.Type == MemberType.Type).Select(m => new DocType(m, xmlModel.Members));
        }
    }
}
