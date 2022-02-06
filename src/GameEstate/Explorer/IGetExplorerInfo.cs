using GameEstate.Formats;
using System.Collections.Generic;

namespace GameEstate.Explorer
{
    public interface IGetExplorerInfo
    {
        List<ExplorerInfoNode> GetInfoNodes(ExplorerManager resource = null, FileMetadata file = null, object tag = null);
    }
}
