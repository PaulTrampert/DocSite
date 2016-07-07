using System;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public class DocEvent
    {
        public MemberDetails MemberDetails { get; }

        public DocEvent(MemberDetails memberDetails)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Event) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Event}", nameof(memberDetails));
            MemberDetails = memberDetails;
        }
    }
}