using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PTrampert.AppArgs.Attributes;

namespace DocSite
{
    public class Arguments
    {
        private string _docXml;

        [Option(Name = "inputFile", ShortName = "i", Description = "The documentation xml file to process.")]
        public string DocXml
        {
            get { return _docXml; }
            set { _docXml = Path.GetFullPath(value); }
        }

        [Option(Name="renderer", ShortName = "r", Description = "The renderer to use. Defaults to Html.")]
        public RendererOptions Renderer { get; set; }

        private string _outDir;
        [Option(Name = "outputDirectory", ShortName = "o", Description = "Directory to render to. Defaults to the current working directory.")]
        public string OutputDirectory
        {
            get { return _outDir; }
            set { _outDir = Path.GetFullPath(value); }
        }

        [Option(Name = "help", ShortName = "h", Description = "Show the help output.")]
        public bool Help { get; set; }

        public Arguments()
        {
            Renderer = RendererOptions.Html;
            OutputDirectory = Directory.GetCurrentDirectory();
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
