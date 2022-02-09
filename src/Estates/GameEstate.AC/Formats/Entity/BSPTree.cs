using GameEstate.AC.Formats.Props;
using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class BSPTree : IGetExplorerInfo
    {
        public readonly BSPNode RootNode;

        public BSPTree(BinaryReader r, BSPType treeType)
            => RootNode = BSPNode.Factory(r, treeType);

        //: Entity.BSPTree
        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"Root", items: (RootNode as IGetExplorerInfo).GetInfoNodes(tag: tag)),
            };
            return nodes;
        }
    }
}
