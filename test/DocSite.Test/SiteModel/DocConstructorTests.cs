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
    public class DocConstructorTests
    {
        [Theory]
        [InlineData("M:Test.Method")]
        [InlineData("P:Test.Property")]
        public void ThrowWhenPassedANonConstructorMemberDetails(string memberId)
        {
            Assert.Throws<ArgumentException>(() => new DocConstructor(new MemberDetails {Id = memberId}));
        }

        [Fact]
        public void ThrowsWhenPassedNullConstructorArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new DocConstructor(null));
        }

        [Theory]
        [InlineData("M:Test.#ctor")]
        [InlineData("M:Test.#ctor(System.String[])")]
        public void DoesNotThrowForConstructorMemberDetails(string memberId)
        {
            var result = new DocConstructor(new MemberDetails {Id = memberId});
            Assert.NotNull(result);
        }
    }
}
