using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEstate.AC.Formats.Entity
{
    public class TerrainDesc : IGetExplorerInfo
    {
        public readonly TerrainType[] TerrainTypes;
        public readonly LandSurf LandSurfaces;

        public TerrainDesc(BinaryReader r)
        {
            TerrainTypes = r.ReadL32Array(x => new TerrainType(x));
            LandSurfaces = new LandSurf(r);
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                new ExplorerInfoNode("TerrainTypes", items: TerrainTypes.Select(x => {
                    var items = (x as IGetExplorerInfo).GetInfoNodes();
                    var name = items[0].Name.Replace("TerrainName: ", "");
                    items.RemoveAt(0);
                    return new ExplorerInfoNode(name, items: items);
                })),
                new ExplorerInfoNode($"LandSurf", items: (LandSurfaces as IGetExplorerInfo).GetInfoNodes()),
            };
            return nodes;
        }
    }
}
