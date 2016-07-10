using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using DocSite.Pages;
using DocSite.SiteModel;

namespace DocSite.Xml
{
    [XmlRoot("member")]
    public class MemberDetails
    {
        public const string IdRegex = @"^[NTFPME!]:(?<fullName>(?<namespace>([^\.\(]+\.)+)(?<localName>[^\(\.]+(\([^\)]+\))?))$";

        [XmlAttribute("name")]
        public string Id {get;set;}

        public string FileId
        {
            get { return Convert.ToBase64String(Encoding.UTF8.GetBytes(Id)).Replace('+', '-').Replace('/', '_').TrimEnd('='); }
        }

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

        public XmlElement Summary => DocXml?.SingleOrDefault(xml => xml.Name == "summary");

        public XmlElement Remarks => DocXml?.SingleOrDefault(xml => xml.Name == "remarks");

        public IEnumerable<XmlElement> Permission => DocXml?.Where(xml => xml.Name == "permission");

        public IEnumerable<XmlElement> Exceptions => DocXml?.Where(xml => xml.Name == "exception");

        public XmlElement Example => DocXml?.SingleOrDefault(xml => xml.Name == "example");

        public IEnumerable<XmlElement> Params => DocXml?.Where(xml => xml.Name == "param");

        public IEnumerable<XmlElement> TypeParams => DocXml?.Where(xml => xml.Name == "typeparam");

        public IEnumerable<XmlElement> SeeAlso => DocXml?.Where(xml => xml.Name == "seealso");

        public XmlElement Returns => DocXml?.SingleOrDefault(xml => xml.Name == "returns");

        public XmlElement Value => DocXml?.SingleOrDefault(xml => xml.Name == "value");

        public void AddCommonSections(IList<ISection> sections)
        {
            AddSummary(sections);
            AddTypeParams(sections);
            AddParams(sections);
            AddReturns(sections);
            AddValue(sections);
            AddExceptions(sections);
            AddPermissions(sections);
            AddRemarks(sections);
            AddExample(sections);
            AddSeeAlso(sections);
        }

        private void AddSeeAlso(IList<ISection> sections)
        {
            if (SeeAlso.Any())
            {
                sections.Add(new Section
                {
                    Title = "See Also",
                    Order = int.MaxValue,
                    Body = SeeAlso
                });
            }
        }

        private void AddValue(IList<ISection> sections)
        {
            if (Value != null)
            {
                sections.Add(new Section
                {
                    Title = "Value",
                    Order = 3,
                    Body = Value.ChildNodes.Cast<XmlNode>()
                });
            }
        }

        private void AddReturns(IList<ISection> sections)
        {
            if (Returns != null)
            {
                sections.Add(new Section
                {
                    Title = "Returns",
                    Order = 3,
                    Body = Returns.ChildNodes.Cast<XmlNode>()
                });
            }
        }

        private void AddExceptions(IList<ISection> sections)
        {
            if (Exceptions.Any())
            {
                sections.Add(new DefinitionsSection
                {
                    Title = "Exceptions",
                    Order = 4,
                    Definitions = Exceptions
                });
            }
        }

        private void AddPermissions(IList<ISection> sections)
        {
            if (Permission.Any())
            {
                sections.Add(new DefinitionsSection
                {
                    Title = "Permissions",
                    Order = 5,
                    Definitions = Permission
                });
            }
        }

        private void AddExample(IList<ISection> sections)
        {
            if (Example != null)
            {
                sections.Add(new Section
                {
                    Title = "Example",
                    Order = 21,
                    Body = Example.ChildNodes.Cast<XmlNode>()
                });
            }
        }

        private void AddRemarks(IList<ISection> sections)
        {
            if (Remarks != null)
            {
                sections.Add(new Section
                {
                    Title = "Remarks",
                    Order = 20,
                    Body = Remarks.ChildNodes.Cast<XmlNode>()
                });
            }
        }

        private void AddSummary(IList<ISection> sections)
        {
            if (Summary != null)
            {
                sections.Add(new Section
                {
                    Title = "Summary",
                    Order = 0,
                    Body = Summary.ChildNodes.Cast<XmlNode>()
                });
            }
        }

        private void AddTypeParams(IList<ISection> sections)
        {
            if (TypeParams.Any())
            {
                sections.Add(new DefinitionsSection()
                {
                    Title = "Type Parameters",
                    Order = 1,
                    Definitions = TypeParams
                });
            }
        }

        private void AddParams(IList<ISection> sections)
        {
            if (Params.Any())
            {
                sections.Add(new DefinitionsSection
                {
                    Title = "Parameters",
                    Order = 2,
                    Definitions = Params
                });
            }
        }

        private Match ParseId()
        {
            return Regex.Match(Id, IdRegex);
        }
    }
}