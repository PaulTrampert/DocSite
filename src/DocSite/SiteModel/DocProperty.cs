using System;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public class DocProperty
    {
        public MemberDetails MemberDetails { get; }

        public DocProperty(MemberDetails memberDetails)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Property) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Property}", nameof(memberDetails));
            MemberDetails = memberDetails;
        }
    }
}