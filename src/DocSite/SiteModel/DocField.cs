using System;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public class DocField
    {
        public MemberDetails MemberDetails { get; }

        public DocField(MemberDetails memberDetails)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Field) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Field}", nameof(memberDetails));
            MemberDetails = memberDetails;
        }
    }
}