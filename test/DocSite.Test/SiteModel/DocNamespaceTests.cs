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
    public class DocNamespaceTests
    {
        [Theory]
        [InlineData("M:Test.Method")]
        [InlineData("P:Test.Property")]
        public void ThrowWhenPassedANonNamespaceMemberDetails(string memberId)
        {
            Assert.Throws<ArgumentException>(() => new DocNamespace(new MemberDetails {Id = memberId}, new List<MemberDetails>()));
        }

        [Fact]
        public void ThrowsWhenPassedNullConstructorArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new DocNamespace(null, new List<MemberDetails>()));
        }

        [Fact]
        public void ThrowsForNullOtherMembersArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new DocNamespace(new MemberDetails(), null));
        }

        [Theory]
        [InlineData("N:Test.OnSomething")]
        [InlineData("N:Test.OnSomethingElse")]
        public void DoesNotThrowForNamespaceMemberDetails(string memberId)
        {
            var result = new DocNamespace(new MemberDetails {Id = memberId}, new List<MemberDetails>());
            Assert.NotNull(result);
        }
    }
}
