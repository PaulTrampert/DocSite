using DocSite.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xunit;

namespace DocSite.Test.Xml
{
    public class AssemblyDetailsTests
    {
        [Fact]
        public void CanDeserializeAssembly()
        {
            var xmlString = @"<assembly><name>PTrampert.AppArgs</name></assembly>";
            var ser = new XmlSerializer(typeof(AssemblyDetails));
            var result = ser.Deserialize(new StringReader(xmlString)) as AssemblyDetails;
            Assert.Equal("PTrampert.AppArgs", result.Name);
        }
    }
}
