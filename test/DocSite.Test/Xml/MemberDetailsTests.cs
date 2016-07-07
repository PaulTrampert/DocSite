using System;
using DocSite.Xml;
using Xunit;
using Xunit.Sdk;
using System.IO;
using System.Xml.Serialization;

namespace DocSite.Test.Xml
{
    public class MemberDetailsTests
    {

        [Theory]
        [InlineData("N:DocSite.Test.Xml", MemberType.Namespace, "DocSite.Test.Xml", "Xml", "DocSite.Test", null)]
        [InlineData("T:DocSite.Test.Xml.MemberDetails", MemberType.Type, "DocSite.Test.Xml.MemberDetails", "MemberDetails", "DocSite.Test.Xml",null)]
        [InlineData("M:DocSite.Test.Xml.MemberDetails.#ctor", MemberType.Method, "DocSite.Test.Xml.MemberDetails.#ctor", "#ctor", "DocSite.Test.Xml.MemberDetails", null)]
        [InlineData("P:DocSite.Test.Xml.MemberDetails.Id", MemberType.Property, "DocSite.Test.Xml.MemberDetails.Id", "Id", "DocSite.Test.Xml.MemberDetails", null)]
        [InlineData("F:DocSite.Test.Xml.MemberDetails.something", MemberType.Field, "DocSite.Test.Xml.MemberDetails.something", "something", "DocSite.Test.Xml.MemberDetails", null)]
        [InlineData("E:DocSite.Test.Xml.MemberDetails.OnEvent", MemberType.Event, "DocSite.Test.Xml.MemberDetails.OnEvent", "OnEvent", "DocSite.Test.Xml.MemberDetails", null)]
        [InlineData("!:SomeErrorOccurred", MemberType.Error, null, null, null, "SomeErrorOccurred")]
        public void ConstructorSetsIdAndDocXml(string id, MemberType expectedType, string expectedFullName, string expectedLocalName, string expectedParent, string expectedError)
        {
            var subject = new MemberDetails {Id = id};
            Assert.Equal(id, subject.Id);
            Assert.Equal(expectedType, subject.Type);
            Assert.Equal(expectedFullName, subject.FullName);
            Assert.Equal(expectedLocalName, subject.LocalName);
            Assert.Equal(expectedParent, subject.ParentMember);
            Assert.Equal(expectedError, subject.Error);
        }

        [Fact]
        public void CanDeserializeMemberDetails()
        {
            var xmlString = @"<member name=""P:DocSite.Test.Xml.MemberDetails.Id""><summary></summary></member>";
            var ser = new XmlSerializer(typeof(MemberDetails));
            var result = ser.Deserialize(new StringReader(xmlString)) as MemberDetails;
            Assert.Equal("P:DocSite.Test.Xml.MemberDetails.Id", result.Id);
            Assert.NotNull(result.DocXml);
        }

    }
}