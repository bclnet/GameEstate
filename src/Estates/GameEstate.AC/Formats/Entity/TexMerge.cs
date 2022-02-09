using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEstate.AC.Formats.Entity
{
    public class TexMerge : IGetExplorerInfo
    {
        public readonly uint BaseTexSize;
        public readonly TerrainAlphaMap[] CornerTerrainMaps;
        public readonly TerrainAlphaMap[] SideTerrainMaps;
        public readonly RoadAlphaMap[] RoadMaps;
        public readonly TMTerrainDesc[] TerrainDesc;

        public TexMerge(BinaryReader r)
        {
            BaseTexSize = r.ReadUInt32();
            CornerTerrainMaps = r.ReadL32Array(x => new TerrainAlphaMap(x));
            SideTerrainMaps = r.ReadL32Array(x => new TerrainAlphaMap(x));
            RoadMaps = r.ReadL32Array(x => new RoadAlphaMap(x));
            TerrainDesc = r.ReadL32Array(x => new TMTerrainDesc(x));
        }

        //: Entity.TexMerge
        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode($"BaseTextureSize: {BaseTexSize}"),
                new ExplorerInfoNode("SideTerrainMap", items: SideTerrainMaps.Select(x => new ExplorerInfoNode($"{x}"))),
                new ExplorerInfoNode("RoadAlphaMap", items: RoadMaps.Select(x => new ExplorerInfoNode($"{x}"))),
                new ExplorerInfoNode("TMTerrainDesc", items: TerrainDesc.Select(x => {
                    var items = (x as IGetExplorerInfo).GetInfoNodes();
                    var name = items[0].Name.Replace("TerrainType: ", "");
                    items.RemoveAt(0);
                    return new ExplorerInfoNode(name, items: items);
                })),
            };
            return nodes;
        }
    }
}
