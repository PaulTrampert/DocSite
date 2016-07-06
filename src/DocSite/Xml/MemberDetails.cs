using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DocSite.Xml
{
    public class MemberDetails
    {
        public const string IdRegex = "^[NTFPME!]:\\S+$";
        public string Id {get;set;}

        public string DocXml {get;set;}

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

        public MemberDetails(string id, string docXml)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (!Regex.IsMatch(id, IdRegex)) throw new ArgumentException($"{nameof(id)} must match {IdRegex}", nameof(id));
            Id = id;
            DocXml = docXml;
        }
    }
}