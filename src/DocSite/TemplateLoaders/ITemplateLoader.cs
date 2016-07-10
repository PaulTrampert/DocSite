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
    }
}