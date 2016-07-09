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
    public class DocFieldTests
    {
        [Theory]
        [InlineData("M:Test.Method")]
        [InlineData("P:Test.Property")]
        public void ThrowWhenPassedANonFieldMemberDetails(string memberId)
        {
            Assert.Throws<ArgumentException>(() => new DocField(new MemberDetails {Id = memberId}));
        }

        [Fact]
        public void ThrowsWhenPassedNullConstructorArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new DocField(null));
        }

        [Theory]
        [InlineData("F:Test.something")]
        [InlineData("F:Test.somethingElse")]
        public void DoesNotThrowForFieldMemberDetails(string memberId)
        {
            var result = new DocField(new MemberDetails {Id = memberId});
            Assert.NotNull(result);
        }
    }
}
