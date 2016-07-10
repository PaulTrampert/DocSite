using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DocSite.Renderers;
using DocSite.Xml;
using DocSite.Pages;

namespace DocSite.SiteModel
{
    /// <summary>
    /// Interface representing the structured documentation model of a member.
    /// </summary>
    /// <seealso cref="DocConstructor"/>
    /// <seealso cref="DocEvent"/>
    /// <seealso cref="DocField"/>
    /// <seealso cref="DocMethod"/>
    /// <seealso cref="DocNamespace"/>
    /// <seealso cref="DocProperty"/>
    /// <seealso cref="DocSiteModel"/>
    /// <seealso cref="DocType"/>
    public interface IDocModel
    {
        /// <summary>
        /// The <see cref="MemberDetails"/> this model was built from.
        /// </summary>
        MemberDetails MemberDetails { get; }

        /// <summary>
        /// The parent <see cref="IDocModel"/>. May be <c>null</c>.
        /// </summary>
        IDocModel Parent { get; }

        /// <summary>
        /// Adds itself and any child members to a dictionary, using <c>MemberDetails.Id</c> as the key.
        /// </summary>
        /// <param name="membersDictionary"></param>
        void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary);

        /// <summary>
        /// Builds a page for this <see cref="IDocModel"/> in the context of a <see cref="DocSiteModel"/>
        /// </summary>
        /// <param name="context">The <see cref="DocSiteModel"/> context to build the page under.</param>
        /// <returns>The renderable <see cref="Page"/></returns>
        Page BuildPage(DocSiteModel context);

        /// <summary>
        /// Builds a <see cref="TableRow"/> for use in a <see cref="TableSection"/>.
        /// </summary>
        /// <returns>The resulting <see cref="TableRow"/></returns>
        TableRow GetTableRow();

        /// <summary>
        /// Builds a tree object.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns><see cref="Tree"/></returns>
        Tree BuildTree(string currentPage);
    }
}
