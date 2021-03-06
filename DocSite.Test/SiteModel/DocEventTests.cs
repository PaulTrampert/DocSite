﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.SiteModel;
using DocSite.Xml;
using Xunit;
using Xunit.Sdk;

namespace DocSite.Test.SiteModel
{
    public class DocEventTests
    {
        [Theory]
        [InlineData("M:Test.Method")]
        [InlineData("P:Test.Property")]
        public void ThrowWhenPassedANonEventMemberDetails(string memberId)
        {
            Assert.Throws<ArgumentException>(() => new DocEvent(new MemberDetails {Id = memberId}));
        }

        [Fact]
        public void ThrowsWhenPassedNullConstructorArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new DocEvent(null));
        }

        [Theory]
        [InlineData("E:Test.OnSomething")]
        [InlineData("E:Test.OnSomethingElse(System.String[])")]
        public void DoesNotThrowForEventMemberDetails(string memberId)
        {
            var result = new DocEvent(new MemberDetails {Id = memberId});
            Assert.NotNull(result);
        }
    }
}
