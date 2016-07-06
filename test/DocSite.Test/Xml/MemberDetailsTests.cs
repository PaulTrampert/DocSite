using System;
using DocSite.Xml;
using Xunit;
using Xunit.Sdk;

namespace DocSite.Test.Xml
{
    public class MemberDetailsTests
    {
        [Fact]
        public void ConstructorThrowsWhenIdIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MemberDetails(null, ""));
            Assert.Equal("id", exception.ParamName);
        }

        [Fact]
        public void ConstructorThrowsWhenIdIsWrongFormat()
        {
            var exception = Assert.Throws<ArgumentException>(() => new MemberDetails("this is not a valid id", ""));
            Assert.Equal("id", exception.ParamName);
            Assert.Equal($"id must match {MemberDetails.IdRegex}\r\nParameter name: id", exception.Message);
        }

        [Theory]
        [InlineData("N:DocSite.Test.Xml", "<summary></summary>", MemberType.Namespace, "DocSite.Test.Xml", "Xml", null)]
        [InlineData("T:DocSite.Test.Xml.MemberDetails", "<summary></summary>", MemberType.Type, "DocSite.Test.Xml.MemberDetails", "MemberDetails", null)]
        [InlineData("M:DocSite.Test.Xml.MemberDetails.#ctor", "<summary></summary>", MemberType.Method, "DocSite.Test.Xml.MemberDetails.#ctor", "#ctor", null)]
        [InlineData("P:DocSite.Test.Xml.MemberDetails.Id", "<summary></summary>", MemberType.Property, "DocSite.Test.Xml.MemberDetails.Id", "Id", null)]
        [InlineData("F:DocSite.Test.Xml.MemberDetails.something", "<summary></summary>", MemberType.Field, "DocSite.Test.Xml.MemberDetails.something", "something", null)]
        [InlineData("E:DocSite.Test.Xml.MemberDetails.OnEvent", "<summary></summary>", MemberType.Event, "DocSite.Test.Xml.MemberDetails.OnEvent", "OnEvent", null)]
        [InlineData("!:SomeErrorOccurred", "<summary></summary>", MemberType.Error, null, null, "SomeErrorOccurred")]
        public void ConstructorSetsIdAndDocXml(string id, string docXml, MemberType expectedType, string expectedFullName, string expectedLocalName, string expectedError)
        {
            var subject = new MemberDetails(id, docXml);
            Assert.Equal(id, subject.Id);
            Assert.Equal(docXml, subject.DocXml);
            Assert.Equal(expectedType, subject.Type);
            Assert.Equal(expectedFullName, subject.FullName);
            Assert.Equal(expectedLocalName, subject.LocalName);
            Assert.Equal(expectedError, subject.Error);
        }
    }
}