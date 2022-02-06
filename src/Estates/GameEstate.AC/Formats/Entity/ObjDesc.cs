using GameEstate.Explorer;
using GameEstate.Formats;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEstate.AC.Formats.Entity
{
    public class ObjDesc : IGetExplorerInfo
    {
        public readonly uint PaletteID;
        public readonly SubPalette[] SubPalettes;
        public readonly TextureMapChange[] TextureChanges;
        public readonly AnimationPartChange[] AnimPartChanges;

        public ObjDesc(BinaryReader r)
        {
            r.AlignBoundary();
            r.ReadByte(); // ObjDesc always starts with 11.
            var numPalettes = r.ReadByte();
            var numTextureMapChanges = r.ReadByte();
            var numAnimPartChanges = r.ReadByte();
            if (numPalettes > 0)
                PaletteID = r.ReadAsDataIDOfKnownType(0x04000000);
            SubPalettes = r.ReadTArray(x => new SubPalette(x), numPalettes);
            TextureChanges = r.ReadTArray(x => new TextureMapChange(x), numTextureMapChanges);
            AnimPartChanges = r.ReadTArray(x => new AnimationPartChange(x), numAnimPartChanges);
            r.AlignBoundary();
        }

        List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        {
            var nodes = new List<ExplorerInfoNode> {
                PaletteID != 0 ? new ExplorerInfoNode($"Palette ID: {PaletteID:X8}") : null,
                SubPalettes.Length > 0 ? new ExplorerInfoNode("SubPalettes", items: SubPalettes.Select(x => {
                    var items = (x as IGetExplorerInfo).GetInfoNodes();
                    var name = items[0].Name.Replace("Skill: ", "");
                    items.RemoveAt(0);
                    return new ExplorerInfoNode(name, items: items);
                })) : null,
                TextureChanges.Length > 0 ? new ExplorerInfoNode("Texture Changes", items: TextureChanges.Select(x => new ExplorerInfoNode($"{x}"))) : null,
                AnimPartChanges.Length > 0 ? new ExplorerInfoNode("AnimPart Changes", items: AnimPartChanges.Select(x => new ExplorerInfoNode($"{x}"))) : null,
            };
            return nodes;
        }
    }
}
