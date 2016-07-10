using DocSite.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DocSite.Test.Pages
{
    public class TreeTests
    {
        [Fact]
        public void SetHrefExtensionSetsTheExtensionForTheWholeTree()
        {
            var tree = new Tree
            {
                Nodes = new List<Tree>
                {
                    new Tree
                    {
                        Nodes = new []
                        {
                            new Tree(), 
                        }
                    },
                    new Tree()
                }
            };

            Assert.Equal(".test", tree.Href);
            Assert.True(tree.Nodes.All(t => t.Href == ".test" && (t.Nodes == null || t.Nodes.All(n2 => n2.Href == ".test"))));
        }
    }
}
