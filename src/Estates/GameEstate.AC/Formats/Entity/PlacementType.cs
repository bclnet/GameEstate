using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;

namespace GameEstate.AC.Formats.Entity
{
    public class PlacementType : IGetExplorerInfo
    {
        public readonly AnimationFrame AnimFrame;

        public PlacementType(BinaryReader r, uint numParts)
        {
            AnimFrame = new AnimationFrame(r, numParts);
        }

        //: Entity.PlacementType
        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag) => (AnimFrame as IGetExplorerInfo).GetInfoNodes(resource, file, tag);
    }
}
