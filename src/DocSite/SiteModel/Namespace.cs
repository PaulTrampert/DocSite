using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public class Namespace
    {
        public MemberDetails MemberDetails { get; }

        public string Name => MemberDetails.FullName;

        public IEnumerable<DocType> Types { get; }

        public Namespace(MemberDetails memberDetails, IEnumerable<MemberDetails> otherMembers)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Namespace) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Namespace}", nameof(memberDetails));
            MemberDetails = memberDetails;
            Types = otherMembers.Where(m => m.Type == MemberType.Type && m.ParentMember == Name).Select(m => new DocType(m, otherMembers));
        }
    }
}
