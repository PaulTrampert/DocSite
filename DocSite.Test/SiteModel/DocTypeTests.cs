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
    public class DocTypeTests
    {
        [Theory]
        [InlineData("M:Test.Method")]
        [InlineData("P:Test.Property")]
        public void ThrowWhenPassedANonTypeMemberDetails(string memberId)
        {
            Assert.Throws<ArgumentException>(() => new DocType(new MemberDetails {Id = memberId}, new List<MemberDetails>()));
        }

        [Fact]
        public void ThrowsWhenPassedNullConstructorArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new DocType(null, new List<MemberDetails>()));
        }

        [Fact]
        public void ThrowsForNullOtherMembersArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new DocType(new MemberDetails(), null));
        }

        [Theory]
        [InlineData("T:Test.OnSomething")]
        [InlineData("T:Test.OnSomethingElse")]
        public void DoesNotThrowForTypeMemberDetails(string memberId)
        {
            var result = new DocType(new MemberDetails {Id = memberId}, new List<MemberDetails>());
            Assert.NotNull(result);
        }
    }
}
