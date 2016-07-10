using System;
using System.Collections.Generic;

namespace DocSite.TemplateLoaders
{
    /// <summary>
    /// Interface for loading template strings.
    /// </summary>
    public interface ITemplateLoader
    {
        /// <summary>
        /// Load template with the given name.
        /// </summary>
        /// <param name="name">Name of the template to load.</param>
        /// <returns><see cref="String"/> - The template string.</returns>
        string LoadTemplate(string name);

        /// <summary>
        /// Load all templates the loader has access to and return by name.
        /// </summary>
        /// <returns>A dictionary of all templates.</returns>
        IDictionary<string, string> LoadAllTemplates();
    }
}