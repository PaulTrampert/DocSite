using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace DocSite.Xml
{
    [XmlRoot("member")]
    public class MemberDetails
    {
        public const string IdRegex = @"^[NTFPME!]:(?<fullName>(?<namespace>([^\.\(]+\.)+)(?<localName>[^\(\.]+(\([^\)]+\))?))$";

        [XmlAttribute("name")]
        public string Id {get;set;}

        [XmlAnyElement]
        public XmlElement[] DocXml {get;set;}

        public MemberType Type
        {
            get
            {
                MemberType result;
                switch (Id[0])
                {
                    case 'N':
                        result = MemberType.Namespace;
                        break;
                    case 'T':
                        result = MemberType.Type;
                        break;
                    case 'F':
                        result = MemberType.Field;
                        break;
                    case 'P':
                        result = MemberType.Property;
                        break;
                    case 'M':
                        result = MemberType.Method;
                        break;
                    case 'E':
                        result = MemberType.Event;
                        break;
                    case '!':
                    default:
                        result = MemberType.Error;
                        break;
                }
                return result;
            }
        }

        public string Error
        {
            get
            {
                if (Type != MemberType.Error) return null;
                return Regex.Match(Id, "!:(.*)").Groups[1].Value;
            }
        }

        public string ParentMember
        {
            get
            {
                if (Type == MemberType.Error) return null;
                return ParseId().Groups["namespace"].Value.TrimEnd('.');
            }
        }

        public string FullName 
        {
            get
            {
                if (Type == MemberType.Error) return null;
                return ParseId().Groups["fullName"].Value;
            }
        }

        public string LocalName 
        {
            get
            {
                if (Type == MemberType.Error) return null;
                return ParseId().Groups["localName"].Value;
            }
        }

        private Match ParseId()
        {
            return Regex.Match(Id, IdRegex);
        }
    }
}