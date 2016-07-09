using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DocSite.Renderers;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public interface IDocModel
    {
        MemberDetails MemberDetails { get; }

        IDocModel Parent { get; }

        void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary);

        Page RenderPage(DocSiteModel context, IRenderer renderer);
    }
}
