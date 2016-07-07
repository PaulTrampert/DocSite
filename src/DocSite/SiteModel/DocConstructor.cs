using System;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public class DocConstructor
    {
        public MemberDetails MemberDetails { get; }

        public DocConstructor(MemberDetails memberDetails)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Method) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Method}", nameof(memberDetails));
            if (!memberDetails.LocalName.StartsWith("#ctor")) throw new ArgumentException($"{nameof(memberDetails)} describes a method", nameof(memberDetails));
            MemberDetails = memberDetails;
        }
    }
}