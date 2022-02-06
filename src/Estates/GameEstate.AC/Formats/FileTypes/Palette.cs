using GameEstate.Explorer;
using GameEstate.Formats;
using OpenStack.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEstate.AC.Formats.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x04. 
    /// </summary>
    [PakFileType(PakFileType.Palette)]
    public class Palette : AbstractFileType, IGetExplorerInfo
    {
        /// <summary>
        /// Color data is stored in ARGB format
        /// </summary>
        public uint[] Colors;

        public Palette(BinaryReader r)
        {
            Id = r.ReadUInt32();
            Colors = r.ReadL32Array<uint>(sizeof(uint));
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"{nameof(Palette)}: {Id:X8}", items: Colors.Select(x => new ExplorerInfoNode(new GXColor(x, GXColor.Format.ARGB32).ToString("F0")))),
            };
            return nodes;
        }
    }
}
