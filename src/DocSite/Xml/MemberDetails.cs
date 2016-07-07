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
        public const string IdRegex = "^[NTFPME!]:\\S+$";

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
                return Id.Substring(Id.IndexOf(':') + 1);
            }
        }

        public string FullName 
        {
            get
            {
                if (Type == MemberType.Error) return null;
                return Id.Substring(Id.IndexOf(':') + 1);
            }
        }

        public string LocalName 
        {
            get
            {
                if (Type == MemberType.Error) return null;
                return Id.Substring(Id.LastIndexOf('.') + 1);
            }
        }
    }
}