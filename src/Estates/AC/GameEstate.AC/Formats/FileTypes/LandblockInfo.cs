using GameEstate.AC.Formats.Entity;
using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEstate.AC.Formats.FileTypes
{
    /// <summary>
    /// This reads the extra items in a landblock from the client_cell.dat. This is mostly buildings, but other static/non-interactive objects like tables, lamps, are also included.
    /// CLandBlockInfo is a file designated xxyyFFFE, where xxyy is the landblock.
    /// <para />
    /// The fileId is CELL + 0xFFFE. e.g. a cell of 1234, the file index would be 0x1234FFFE.
    /// </summary>
    /// <remarks>
    /// Very special thanks again to David Simpson for his early work on reading the cell.dat. Even bigger thanks for his documentation of it!
    /// </remarks>
    [PakFileType(PakFileType.LandBlockInfo)]
    public class LandblockInfo : FileType, IGetExplorerInfo
    {
        /// <summary>
        /// number of EnvCells in the landblock. This should match up to the unique items in the building stab lists.
        /// </summary>
        public readonly uint NumCells;
        /// <summary>
        /// list of model numbers. 0x01 and 0x02 types and their specific locations
        /// </summary>
        public readonly Stab[] Objects;
        /// <summary>
        /// As best as I can tell, this only affects whether there is a restriction table or not
        /// </summary>
        public readonly uint PackMask;
        /// <summary>
        /// Buildings and other structures with interior locations in the landblock
        /// </summary>
        public readonly BuildInfo[] Buildings;
        /// <summary>
        /// The specific landblock/cell controlled by a specific guid that controls access (e.g. housing barrier)
        /// </summary>
        public readonly Dictionary<uint, uint> RestrictionTables;

        public LandblockInfo(BinaryReader r)
        {
            Id = r.ReadUInt32();
            NumCells = r.ReadUInt32();
            Objects = r.ReadL32Array(x => new Stab(x));
            var numBuildings = r.ReadUInt16();
            PackMask = r.ReadUInt16();
            Buildings = r.ReadTArray(x => new BuildInfo(x), numBuildings);
            if ((PackMask & 1) == 1) RestrictionTables = r.ReadL16Many<uint, uint>(sizeof(uint), x => x.ReadUInt32(), offset: 2);
        }

        //: FileTypes.LandblockInfo
        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var landblock = Id & 0xFFFF0000;
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"{nameof(LandblockInfo)}: {Id:X8}", items: new List<ExplorerInfoNode> {
                    new ExplorerInfoNode($"NumCells: {NumCells}"),
                    NumCells > 0 ? new ExplorerInfoNode("Objects", items: Enumerable.Range(0, (int)NumCells).Select(i => new ExplorerInfoNode($"{landblock + 0x100 + i:X8}", clickable: true))) : null,
                    Objects.Length > 0 ? new ExplorerInfoNode("Objects", items: Objects.Select(x => {
                        var items = (x as IGetExplorerInfo).GetInfoNodes();
                        var name = items[0].Name.Replace("ID: ", "");
                        items.RemoveAt(0);
                        return new ExplorerInfoNode(name, items: items, clickable: true);
                    })) : null,
                    //PackMask != 0 ? new ExplorerInfoNode($"PackMask: {PackMask}") : null,
                    Buildings.Length > 0 ? new ExplorerInfoNode("Buildings", items: Buildings.Select((x, i) => new ExplorerInfoNode($"{i}", items: (x as IGetExplorerInfo).GetInfoNodes()))) : null,
                    RestrictionTables.Count > 0 ? new ExplorerInfoNode("Restrictions", items: RestrictionTables.Select(x => new ExplorerInfoNode($"{x.Key:X8}: {x.Value:X8}"))) : null,
                })
            };
            return nodes;
        }
    }
}
