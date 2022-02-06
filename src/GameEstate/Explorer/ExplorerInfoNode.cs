using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameEstate.Explorer
{
    [DebuggerDisplay("{Name}, items: {Items.Count} [{Tag}]")]
    public class ExplorerInfoNode
    {
        public string Name { get; set; }
        public object Tag { get; }
        public IEnumerable<ExplorerInfoNode> Items { get; }

        public ExplorerInfoNode(string name, object tag = null, IEnumerable<ExplorerInfoNode> items = null)
        {
            Name = name;
            Tag = tag;
            Items = items ?? Enumerable.Empty<ExplorerInfoNode>();
        }
    }
}
