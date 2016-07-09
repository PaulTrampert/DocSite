using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.SiteModel;
using DocSite.Xml;
using Xunit;
using Xunit.Sdk;

namespace DocSite.Test.SiteModel
{
    public class DocPropertyTests
    {
        [Theory]
        [InlineData("M:Test.Method")]
        [InlineData("F:Test.Field")]
        public void ThrowWhenPassedANonPropertyMemberDetails(string memberId)
        {
            Assert.Throws<ArgumentException>(() => new DocProperty(new MemberDetails {Id = memberId}));
        }

        [Fact]
        public void ThrowsWhenPassedNullConstructorArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new DocProperty(null));
        }

        [Theory]
        [InlineData("P:Test.Something")]
        [InlineData("P:Test.SomethingElse")]
        public void DoesNotThrowForPropertyMemberDetails(string memberId)
        {
            var result = new DocProperty(new MemberDetails {Id = memberId});
            Assert.NotNull(result);
        }
    }
}
