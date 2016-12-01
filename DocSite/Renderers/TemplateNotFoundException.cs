using System;

namespace DocSite.Renderers
{
    /// <summary>
    /// Exception indicating that the template was not found.
    /// </summary>
    internal class TemplateNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="TemplateNotFoundException"/>.
        /// </summary>
        /// <param cref="string" name="templateName">Name of the template that couldn't be found.</param>
        public TemplateNotFoundException(string templateName) : base($"{templateName} could not be found")
        {
        }
    }
}