using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEstate.AC.Formats.Entity
{
    public class CloSubPalette : IGetExplorerInfo
    {
        /// <summary>
        /// Contains a list of valid offsets & color values
        /// </summary>
        public readonly CloSubPaletteRange[] Ranges;
        /// <summary>
        /// Icon portal.dat 0x0F000000
        /// </summary>
        public readonly uint PaletteSet;

        public CloSubPalette(BinaryReader r)
        {
            Ranges = r.ReadL32Array(x => new CloSubPaletteRange(x));
            PaletteSet = r.ReadUInt32();
        }

        //: Entity.ClothingSubPalette
        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                Ranges.Length == 1
                    ? new ExplorerInfoNode($"Range: {Ranges[0]}")
                    : new ExplorerInfoNode($"SubPalette Ranges", items: Ranges.Select(x => new ExplorerInfoNode($"{x}"))),
                new ExplorerInfoNode($"Palette Set: {PaletteSet:X8}", clickable: true),
            };
            return nodes;
        }
    }
}
