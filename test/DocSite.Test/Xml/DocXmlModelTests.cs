using DocSite.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DocSite.SiteModel;
using Xunit;

namespace DocSite.Test.Xml
{
    public class DocXmlModelTests
    {

        [Fact]
        public void CanDeserializeDocSiteModel()
        {
            var ser = new XmlSerializer(typeof(DocXmlModel));
            var result = ser.Deserialize(new StringReader(XmlString)) as DocXmlModel;
            Assert.Equal("PTrampert.AppArgs", result.Assembly.Name);
            Assert.NotEmpty(result.Members);
        }

        [Fact]
        public void CanBuildDocSiteModelFromDocXmlModel()
        {
            var ser = new XmlSerializer(typeof(DocXmlModel));
            var result = ser.Deserialize(new StringReader(XmlString)) as DocXmlModel;
            var siteModel = new DocSiteModel(result);
            Assert.Equal("PTrampert.AppArgs", siteModel.AssemblyName);
        }

        private const string XmlString = @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>PTrampert.AppArgs</name>
    </assembly>
    <members>
        <member name=""T:PTrampert.AppArgs.ArgumentParser`1"">
            <summary>
            Parser for parsing ordered arguments.
            </summary>
        </member>
        <member name=""M:PTrampert.AppArgs.ArgumentParser`1.#ctor"">
            <summary>
            Initializes an argument parser.
            </summary>
        </member>
        <member name=""M:PTrampert.AppArgs.ArgumentParser`1.Parse(System.String[])"">
            <summary>
            Parse ordered arguments into the given object.
            </summary>
            <param name=""args"">Command line arguments.</param>
            <returns>The populated object.</returns>
        </member>
        <member name=""M:PTrampert.AppArgs.ArgumentParser`1.Parse(System.String[],`0)"">
            <summary>
            Parse ordered arguments into the given object.
            </summary>
            <param name=""args"">Command line arguments.</param>
            <param name=""obj"">The object to store the arguments in.</param>
            <returns>The populated object.</returns>
        </member>
        <member name=""T:PTrampert.AppArgs.HelpBuilder`1"">
            <summary>
            Class for building help output.
            </summary>
        </member>
        <member name=""M:PTrampert.AppArgs.HelpBuilder`1.BuildHelp(System.String)"">
            <summary>
            Builds help output for the object type.
            </summary>
            <param name=""exeName"">Name of the executable</param>
            <returns>Help output.</returns>
        </member>
        <member name=""T:PTrampert.AppArgs.ICliParser`1"">
            <summary>
            Interface for a command line interface parser.
            </summary>
        </member>
        <member name=""M:PTrampert.AppArgs.ICliParser`1.Parse(System.String[])"">
            <summary>
            Parses command line args array into a new instance of the generic type <c>T</c>
            </summary>
            <param name=""args"">Command line args array passed into Main</param>
            <returns>Populated instance of <c>T</c></returns>
        </member>
        <member name=""M:PTrampert.AppArgs.ICliParser`1.Parse(System.String[],`0)"">
            <summary>
            Parses command line args array into an existing instance of the generic type <c>T</c>
            </summary>
            <param name=""args"">Command line args array passed into Main</param>
            <param name=""obj"">Instance to store parsed results into</param>
            <returns>Populated instance of <c>T</c></returns>
        </member>
        <member name=""T:PTrampert.AppArgs.MixedParser`1"">
            <summary>
            A cli parser that allows the usage of multiple parsers.
            </summary>
        </member>
        <member name=""M:PTrampert.AppArgs.MixedParser`1.#ctor"">
            <summary>
            Default constructor that initializes the MixedParser with an <c>ArgumentParser&lt;T&gt;</c> and an <c>OptionParser&lt;T&gt;</c>
            </summary>
        </member>
        <member name=""M:PTrampert.AppArgs.MixedParser`1.#ctor(System.Collections.Generic.IEnumerable{PTrampert.AppArgs.ICliParser{`0}})"">
            <summary>
            Constructor that allows the passing in of a custom collection of sub parsers.
            </summary>
            <param name=""subParsers"">Consumer specified collection of sub parsers.</param>
        </member>
        <member name=""M:PTrampert.AppArgs.MixedParser`1.Parse(System.String[])"">
            <summary>
            Parses command line args array into a new instance of the generic type <c>T</c>
            </summary>
            <param name=""args"">Command line args array passed into Main</param>
            <returns>Populated instance of <c>T</c></returns>
        </member>
        <member name=""M:PTrampert.AppArgs.MixedParser`1.Parse(System.String[],`0)"">
            <summary>
            Parses command line args array into an existing instance of the generic type <c>T</c>
            </summary>
            <param name=""args"">Command line args array passed into Main</param>
            <param name=""obj"">Instance to store parsed results into</param>
            <returns>Populated instance of <c>T</c></returns>
        </member>
        <member name=""T:PTrampert.AppArgs.OptionParser`1"">
            <summary>
            Parser for parsing command line options.
            </summary>
        </member>
        <member name=""M:PTrampert.AppArgs.OptionParser`1.#ctor(System.Boolean)"">
            <summary>
            Initializes an option parser.
            </summary>
            <param name=""strict"">Whether or not strict parsing rules should be used. Defaults to true.</param>
        </member>
        <member name=""M:PTrampert.AppArgs.OptionParser`1.Parse(System.String[])"">
            <summary>
            Parse command line options into the given object.
            </summary>
            <param name=""args"">Command line arguments.</param>
            <returns>The populated object.</returns>
        </member>
        <member name=""M:PTrampert.AppArgs.OptionParser`1.Parse(System.String[],`0)"">
            <summary>
            Parse command line options into the given object.
            </summary>
            <param name=""args"">Command line arguments.</param>
            <param name=""obj"">The object to store the options in.</param>
            <returns>The populated object.</returns>
        </member>
        <member name=""T:PTrampert.AppArgs.Attributes.ArgumentAttribute"">
            <summary>
            Marks a property as a command line argument. Command line arguments must come in the same order, designated by Order, and must always occurr before Options.
            Required arguments must always come before optional arguments.
            </summary>
        </member>
        <member name=""P:PTrampert.AppArgs.Attributes.ArgumentAttribute.Order"">
            <summary>
            Integer specifying what order the argument occurrs in.
            </summary>
        </member>
        <member name=""P:PTrampert.AppArgs.Attributes.ArgumentAttribute.IsRequired"">
            <summary>
            Boolean specifying whether the argument is optional or not. Defaults to true.
            </summary>
        </member>
        <member name=""P:PTrampert.AppArgs.Attributes.ArgumentAttribute.Name"">
            <summary>
            A human readable display name for the argument. Used in generated -h|-help documentation and error messages.
            </summary>
        </member>
        <member name=""P:PTrampert.AppArgs.Attributes.ArgumentAttribute.Description"">
            <summary>
            A description of the argument's purpose. Used in the auto-generated help text. 
            </summary>
        </member>
        <member name=""M:PTrampert.AppArgs.Attributes.ArgumentAttribute.#ctor(System.Int32)"">
            <summary>
            Marks a property as a command line argument.
            </summary>
            <param name=""order"">The position at which to find the argument in the args array.</param>
        </member>
        <member name=""T:PTrampert.AppArgs.Attributes.OptionAttribute"">
            <summary>
            Attribute specifying an option. Options can come in any order, but must always have either a Name, a ShortName, or both.
            When specified by the command line, an options Name must be preceded by '--', and its short name preceded by '-'. When
            an OptionAttribute is applied to a bool property, it will be treated as a flag.
            </summary>
            <example>
            An option specified with the following attribute:
            <c>
            [Option(Name = ""test"", ShortName = 't')]
            </c>
            Would be specified in the command line by either <c>--test</c> or <c>-t</c>.
            </example>
        </member>
        <member name=""P:PTrampert.AppArgs.Attributes.OptionAttribute.Name"">
            <summary>
            Name of the option. To use this option from the command line, you would call it with <c>-Name</c>
            </summary>
        </member>
        <member name=""P:PTrampert.AppArgs.Attributes.OptionAttribute.ShortName"">
            <summary>
            Short name of the option. To use this option from the command line, you would call it with <c>-ShortName</c>
            </summary>
        </member>
        <member name=""P:PTrampert.AppArgs.Attributes.OptionAttribute.Description"">
            <summary>
            A short description of the option for use in the <c>-h|-help</c> output.
            </summary>
        </member>
        <member name=""T:PTrampert.AppArgs.Exceptions.MissingArgumentException"">
            <summary>
            Thrown when an argument is not found in the args array.
            </summary>
        </member>
        <member name=""T:PTrampert.AppArgs.Exceptions.ParsingException"">
            <summary>
            Exception thrown when an argument can't be parsed.
            </summary>
        </member>
        <member name=""T:PTrampert.AppArgs.Exceptions.RequiredArgumentAfterOptionalException"">
            <summary>
            Thrown when an optional argument occurrs before a required argument.
            </summary>
        </member>
        <member name=""T:PTrampert.AppArgs.Exceptions.UnparseableArgumentException"">
            <summary>
            Thrown when the argument is unparseable.
            </summary>
        </member>
        <member name=""T:PTrampert.AppArgs.Exceptions.UnparseableOptionException"">
            <summary>
            Thrown when the option is unparseable.
            </summary>
        </member>
        <member name=""T:PTrampert.AppArgs.Exceptions.UnrecognizedOptionException"">
            <summary>
            Thrown when an unrecognized option is encountered.
            </summary>
        </member>
        <member name=""M:PTrampert.AppArgs.Extensions.TypeExtensions.GetParseMethod(System.Type)"">
            <summary>
            Returns a parsing function for the given <c>Type</c>
            </summary>
            <param name=""type"">The type.</param>
            <returns>A method to parse the type from a string.</returns>
        </member>
    </members>
</doc>
";
    }
}