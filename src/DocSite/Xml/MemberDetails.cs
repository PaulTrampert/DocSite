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
    /// <summary>
    /// Represents the member element in a documentation xml document.
    /// </summary>
    [XmlRoot("member")]
    public class MemberDetails
    {
        /// <summary>
        /// Regex for parsing the <see cref="Id"/>
        /// </summary>
        public const string IdRegex = @"^[NTFPME!]:(?<fullName>(?<namespace>([^\.\(]+\.)+)?(?<localName>[^\(\.]+(\([^\)]+\))?))$";

        /// <summary>
        /// The Id of the member. Mapps to the name attribute in the xml document.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Id"/></value>
        [XmlAttribute("name")]
        public string Id {get;set;}

        /// <summary>
        /// A filesystem friendly id for the member.
        /// </summary>
        /// <value>Gets the <see cref="FileId"/></value>
        public string FileId
        {
            get
            {
                using (var md5 = MD5.Create())
                {
                    return
                        Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(Id)))
                            .Replace('+', '-')
                            .Replace('/', '_')
                            .TrimEnd('=');
                }
            }
        }

        /// <summary>
        /// Raw xml data contained within the member element.
        /// </summary>
        /// <value>Gets/Sets the <see cref="DocXml"/></value>
        [XmlAnyElement]
        public XmlElement[] DocXml {get;set;}

        /// <summary>
        /// The type of member being represented.
        /// </summary>
        /// <value>Gets the <see cref="Type"/></value>
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

        /// <summary>
        /// If member type is error, this will be the error message.
        /// </summary>
        /// <value>Gets the <see cref="Error"/></value>
        public string Error
        {
            get
            {
                if (Type != MemberType.Error) return null;
                return Regex.Match(Id, "!:(.*)").Groups[1].Value;
            }
        }

        /// <summary>
        /// The full name of what is likely the parent member of this member.
        /// </summary>
        /// <value>Gets the <see cref="ParentMember"/></value>
        public string ParentMember
        {
            get
            {
                if (Type == MemberType.Error) return null;
                return ParseId().Groups["namespace"].Value.TrimEnd('.');
            }
        }

        /// <summary>
        /// The full name of this member.
        /// </summary>
        /// <value>Gets the full name of this member.</value>
        public string FullName 
        {
            get
            {
                if (Type == MemberType.Error) return null;
                return ParseId().Groups["fullName"].Value;
            }
        }

        /// <summary>
        /// The short, or local name of this member.
        /// </summary>
        /// <value>Gets the <see cref="LocalName"/></value>
        public string LocalName 
        {
            get
            {
                if (Type == MemberType.Error) return null;
                return ParseId().Groups["localName"].Value;
            }
        }

        /// <summary>
        /// Gets the summary element.
        /// </summary>
        public XmlElement Summary => DocXml?.SingleOrDefault(xml => xml.Name == "summary");

        /// <summary>
        /// Gets the remarks element.
        /// </summary>
        public XmlElement Remarks => DocXml?.SingleOrDefault(xml => xml.Name == "remarks");

        /// <summary>
        /// Gets all permission elements.
        /// </summary>
        public IEnumerable<XmlElement> Permission => DocXml?.Where(xml => xml.Name == "permission");

        /// <summary>
        /// Gets all exception elements.
        /// </summary>
        public IEnumerable<XmlElement> Exceptions => DocXml?.Where(xml => xml.Name == "exception");

        /// <summary>
        /// Gets the example element.
        /// </summary>
        public XmlElement Example => DocXml?.SingleOrDefault(xml => xml.Name == "example");

        /// <summary>
        /// Gets the params elements.
        /// </summary>
        public IEnumerable<XmlElement> Params => DocXml?.Where(xml => xml.Name == "param");

        /// <summary>
        /// Gets the typeparam elements.
        /// </summary>
        public IEnumerable<XmlElement> TypeParams => DocXml?.Where(xml => xml.Name == "typeparam");

        /// <summary>
        /// Gets the seealso elements.
        /// </summary>
        public IEnumerable<XmlElement> SeeAlso => DocXml?.Where(xml => xml.Name == "seealso");

        /// <summary>
        /// Gets the returns element.
        /// </summary>
        public XmlElement Returns => DocXml?.SingleOrDefault(xml => xml.Name == "returns");

        /// <summary>
        /// Gets the value element.
        /// </summary>
        public XmlElement Value => DocXml?.SingleOrDefault(xml => xml.Name == "value");

        /// <summary>
        /// Adds top level sections to the provided list of sections.
        /// </summary>
        /// <param name="sections">List of sections to add to.</param>
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
            if (SeeAlso != null && SeeAlso.Any())
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
            if (Exceptions != null && Exceptions.Any())
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
            if (Permission != null && Permission.Any())
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
            if (TypeParams != null && TypeParams.Any())
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
            if (Params != null && Params.Any())
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