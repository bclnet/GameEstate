using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEstate.AC.Formats.FileTypes
{
    [PakFileType(PakFileType.BadData)]
    public class BadData : FileType, IGetExplorerInfo
    {
        public const uint FILE_ID = 0x0E00001A;

        // Key is a list of a WCIDs that are "bad" and should not exist. The value is always 1 (could be a bool?)
        public readonly Dictionary<uint, uint> Bad;

        public BadData(BinaryReader r)
        {
            Id = r.ReadUInt32();
            Bad = r.ReadL16Many<uint, uint>(sizeof(uint), x => x.ReadUInt32(), offset: 2);
        }

        //: FileTypes.BadData
        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"{nameof(BadData)}: {Id:X8}", items: Bad.Keys.OrderBy(x => x).Select(x => new ExplorerInfoNode($"{x}")))
            };
            return nodes;
        }
    }
}
