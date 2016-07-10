using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DocSite.Renderers;
using DocSite.Xml;
using DocSite.Pages;

namespace DocSite.SiteModel
{
    public interface IDocModel
    {
        MemberDetails MemberDetails { get; }

        IDocModel Parent { get; }

        void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary);

        Page BuildPage(DocSiteModel context);

        TableRow GetTableRow();
    }
}
