using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public interface IDocModel
    {
        MemberDetails MemberDetails { get; }

        IDocModel Parent { get; }

        void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary);
    }
}
