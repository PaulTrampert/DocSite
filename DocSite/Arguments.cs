using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DocSite
{
    /// <summary>
    /// Object specifying the arguments for the application.
    /// </summary>
    public class Arguments
    {
        private string _docXml;

        /// <summary>
        /// The input file. Setting this converts the value to a full path.
        /// </summary>
        /// <value>Gets/Sets the <see cref="DocXml"/></value>
        public string DocXml
        {
            get { return _docXml; }
            set { _docXml = Path.GetFullPath(value); }
        }

        /// <summary>
        /// The renderer to use.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Renderer"/></value>
        public RendererOptions Renderer { get; set; }

        private string _outDir;

        /// <summary>
        /// The directory to render the site to. This is expanded to a full path.
        /// </summary>
        /// <value>Gets/Sets the <see cref="OutputDirectory"/></value>
        public string OutputDirectory
        {
            get { return _outDir; }
            set { _outDir = Path.GetFullPath(value); }
        }

        /// <summary>
        /// Property specifying the minimum LogLevel to output. Default is Information.
        /// </summary>
        /// <value>Gets/Sets the <see cref="LogLevel"/></value>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Creates a new Arguments class with default values.
        /// </summary>
        public Arguments()
        {
            Renderer = RendererOptions.Html;
            OutputDirectory = Directory.GetCurrentDirectory();
            LogLevel = LogLevel.Information;
        }
    }

    /// <summary>
    /// Available renderer types.
    /// </summary>
    public enum RendererOptions
    {
        /// <summary>
        /// Specifies the Html renderer type.
        /// </summary>
        Html
    }
}
