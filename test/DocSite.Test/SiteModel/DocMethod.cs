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
    public class DocMethodTests
    {
        [Theory]
        [InlineData("F:Test.Method")]
        [InlineData("P:Test.Property")]
        [InlineData("M:Test.#ctor")]
        public void ThrowWhenPassedANonMethodMemberDetails(string memberId)
        {
            Assert.Throws<ArgumentException>(() => new DocMethod(new MemberDetails {Id = memberId}));
        }

        [Fact]
        public void ThrowsWhenPassedNullConstructorArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new DocMethod(null));
        }

        [Theory]
        [InlineData("M:Test.Something")]
        [InlineData("M:Test.SomethingElse(System.String[])")]
        public void DoesNotThrowForMethodMemberDetails(string memberId)
        {
            var result = new DocMethod(new MemberDetails {Id = memberId});
            Assert.NotNull(result);
        }
    }
}
