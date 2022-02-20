using GameEstate.AC.Formats.FileTypes;
using GameEstate.AC.Formats.Props;
using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;
using Environment = GameEstate.AC.Formats.FileTypes.Environment;

namespace GameEstate.AC.Formats
{
    /// <summary>
    /// FormatExtensions
    /// </summary>
    public static class FormatExtensions
    {
        internal static string GetPath(this FileMetadata source, BinaryReader r, PakType pakType, out PakFileType? fileType)
        {
            if ((uint)source.Id == Iteration.FILE_ID) { fileType = null; return "Iteration"; }
            var (type, ext) = source.GetFileType(pakType);
            if (type == 0) { fileType = null; return $"{source.Id:X8}"; }
            fileType = type;
            return ext switch
            {
                null => $"{fileType}/{source.Id:X8}",
                string extension => $"{fileType}/{source.Id:X8}.{extension}",
                Func<FileMetadata, BinaryReader, string> func => $"{fileType}/{source.Id:X8}.{func(source, r)}",
                _ => throw new ArgumentOutOfRangeException(nameof(ext), ext.ToString()),
            };
        }

        // object factory
        internal static Func<BinaryReader, FileMetadata, Task<object>> GetObjectFactory(this FileMetadata source, PakType pakType, PakFileType? type)
        {
            if ((uint)source.Id == Iteration.FILE_ID) return (r, m) => Task.FromResult((object)new Iteration(r));
            else if (type == null) return null;
            else return type.Value switch
            {
                PakFileType.LandBlock => (r, m) => Task.FromResult((object)new Landblock(r)),
                PakFileType.LandBlockInfo => (r, m) => Task.FromResult((object)new LandblockInfo(r)),
                PakFileType.EnvCell => (r, m) => Task.FromResult((object)new EnvCell(r)),
                //PakFileType.LandBlockObjects => null;
                //PakFileType.Instantiation => null;
                PakFileType.GraphicsObject => (r, m) => Task.FromResult((object)new GfxObj(r)),
                PakFileType.Setup => (r, m) => Task.FromResult((object)new SetupModel(r)),
                PakFileType.Animation => (r, m) => Task.FromResult((object)new Animation(r)),
                //PakFileType.AnimationHook => null;
                PakFileType.Palette => (r, m) => Task.FromResult((object)new Palette(r)),
                PakFileType.SurfaceTexture => (r, m) => Task.FromResult((object)new SurfaceTexture(r)),
                PakFileType.Texture => (r, m) => Task.FromResult((object)new Texture(r)),
                PakFileType.Surface => (r, m) => Task.FromResult((object)new Surface(r)),
                PakFileType.MotionTable => (r, m) => Task.FromResult((object)new MotionTable(r)),
                PakFileType.Wave => (r, m) => Task.FromResult((object)new Wave(r)),
                PakFileType.Environment => (r, m) => Task.FromResult((object)new Environment(r)),
                PakFileType.ChatPoseTable => (r, m) => Task.FromResult((object)new ChatPoseTable(r)),
                PakFileType.ObjectHierarchy => (r, m) => Task.FromResult((object)new GeneratorTable(r)), //: Name wayoff
                PakFileType.BadData => (r, m) => Task.FromResult((object)new BadData(r)),
                PakFileType.TabooTable => (r, m) => Task.FromResult((object)new TabooTable(r)),
                PakFileType.FileToId => null,
                PakFileType.NameFilterTable => (r, m) => Task.FromResult((object)new NameFilterTable(r)),
                PakFileType.MonitoredProperties => null,
                PakFileType.PaletteSet => (r, m) => Task.FromResult((object)new PaletteSet(r)),
                PakFileType.Clothing => (r, m) => Task.FromResult((object)new ClothingTable(r)),
                PakFileType.DegradeInfo => (r, m) => Task.FromResult((object)new GfxObjDegradeInfo(r)),
                PakFileType.Scene => (r, m) => Task.FromResult((object)new Scene(r)),
                PakFileType.Region => (r, m) => Task.FromResult((object)new RegionDesc(r)),
                PakFileType.KeyMap => null,
                PakFileType.RenderTexture => (r, m) => Task.FromResult((object)new RenderTexture(r)),
                PakFileType.RenderMaterial => null,
                PakFileType.MaterialModifier => null,
                PakFileType.MaterialInstance => null,
                PakFileType.SoundTable => (r, m) => Task.FromResult((object)new SoundTable(r)),
                PakFileType.UILayout => null,
                PakFileType.EnumMapper => (r, m) => Task.FromResult((object)new EnumMapper(r)),
                PakFileType.StringTable => (r, m) => Task.FromResult((object)new StringTable(r)),
                PakFileType.DidMapper => (r, m) => Task.FromResult((object)new DidMapper(r)),
                PakFileType.ActionMap => null,
                PakFileType.DualDidMapper => (r, m) => Task.FromResult((object)new DualDidMapper(r)),
                PakFileType.String => (r, m) => Task.FromResult((object)new LanguageString(r)), //: Name wayoff
                PakFileType.ParticleEmitter => (r, m) => Task.FromResult((object)new ParticleEmitterInfo(r)),
                PakFileType.PhysicsScript => (r, m) => Task.FromResult((object)new PhysicsScript(r)),
                PakFileType.PhysicsScriptTable => (r, m) => Task.FromResult((object)new PhysicsScriptTable(r)),
                PakFileType.MasterProperty => null,
                PakFileType.Font => (r, m) => Task.FromResult((object)new Font(r)),
                PakFileType.FontLocal => null,
                PakFileType.StringState => (r, m) => Task.FromResult((object)new LanguageInfo(r)), //: Name wayoff
                PakFileType.DbProperties => null,
                PakFileType.RenderMesh => null,
                PakFileType.WeenieDefaults => null,
                PakFileType.CharacterGenerator => (r, m) => Task.FromResult((object)new CharGen(r)),
                PakFileType.SecondaryAttributeTable => (r, m) => Task.FromResult((object)new SecondaryAttributeTable(r)),
                PakFileType.SkillTable => (r, m) => Task.FromResult((object)new SkillTable(r)),
                PakFileType.SpellTable => (r, m) => Task.FromResult((object)new SpellTable(r)),
                PakFileType.SpellComponentTable => (r, m) => Task.FromResult((object)new SpellComponentTable(r)),
                PakFileType.TreasureTable => null,
                PakFileType.CraftTable => null,
                PakFileType.XpTable => (r, m) => Task.FromResult((object)new XpTable(r)),
                PakFileType.Quests => null,
                PakFileType.GameEventTable => null,
                PakFileType.QualityFilter => (r, m) => Task.FromResult((object)new QualityFilter(r)),
                PakFileType.CombatTable => (r, m) => Task.FromResult((object)new CombatManeuverTable(r)),
                PakFileType.ItemMutation => null,
                PakFileType.ContractTable => (r, m) => Task.FromResult((object)new ContractTable(r)),
                _ => null,
            };
        }

        public static (PakFileType fileType, object ext) GetFileType(this FileMetadata source, PakType pakType)
        {
            var objectId = (uint)source.Id;
            if (pakType == PakType.Cell)
            {
                if ((objectId & 0xFFFF) == 0xFFFF) return (PakFileType.LandBlock, "land");
                else if ((objectId & 0xFFFF) == 0xFFFE) return (PakFileType.LandBlockInfo, "lbi");
                else return (PakFileType.EnvCell, "cell");
            }
            else if (pakType == PakType.Portal)
            {
                switch (objectId >> 24)
                {
                    case 0x01: return (PakFileType.GraphicsObject, "obj");
                    case 0x02: return (PakFileType.Setup, "set");
                    case 0x03: return (PakFileType.Animation, "anm");
                    case 0x04: return (PakFileType.Palette, "pal");
                    case 0x05: return (PakFileType.SurfaceTexture, "texture");
                    case 0x06: return (PakFileType.Texture, new PakFileExtensionAttribute(typeof(FormatExtensions), "TextureExtensionLookup").Value);
                    case 0x08: return (PakFileType.Surface, "surface");
                    case 0x09: return (PakFileType.MotionTable, "dsc");
                    case 0x0A: return (PakFileType.Wave, "wav");
                    case 0x0D: return (PakFileType.Environment, "env");
                    case 0x0F: return (PakFileType.PaletteSet, "pst");
                    case 0x10: return (PakFileType.Clothing, "clo");
                    case 0x11: return (PakFileType.DegradeInfo, "deg");
                    case 0x12: return (PakFileType.Scene, "scn");
                    case 0x13: return (PakFileType.Region, "rgn");
                    case 0x14: return (PakFileType.KeyMap, "keymap");
                    case 0x15: return (PakFileType.RenderTexture, "rtexture");
                    case 0x16: return (PakFileType.RenderMaterial, "mat");
                    case 0x17: return (PakFileType.MaterialModifier, "mm");
                    case 0x18: return (PakFileType.MaterialInstance, "mi");
                    case 0x20: return (PakFileType.SoundTable, "stb");
                    case 0x22: return (PakFileType.EnumMapper, "emp");
                    case 0x25: return (PakFileType.DidMapper, "imp");
                    case 0x26: return (PakFileType.ActionMap, "actionmap");
                    case 0x27: return (PakFileType.DualDidMapper, "dimp");
                    case 0x30: return (PakFileType.CombatTable, null);
                    case 0x31: return (PakFileType.String, "str");
                    case 0x32: return (PakFileType.ParticleEmitter, "emt");
                    case 0x33: return (PakFileType.PhysicsScript, "pes");
                    case 0x34: return (PakFileType.PhysicsScriptTable, "pet");
                    case 0x39: return (PakFileType.MasterProperty, "mpr");
                    case 0x40: return (PakFileType.Font, "font");
                    case 0x78: return (PakFileType.DbProperties, new PakFileExtensionAttribute(typeof(FormatExtensions), "DbPropertyExtensionLookup").Value);
                }
                switch (objectId >> 16)
                {
                    case 0x0E01: return (PakFileType.QualityFilter, null);
                    case 0x0E02: return (PakFileType.MonitoredProperties, "monprop");
                }
                if (objectId == 0x0E000002) return (PakFileType.CharacterGenerator, null);
                else if (objectId == 0x0E000003) return (PakFileType.SecondaryAttributeTable, null);
                else if (objectId == 0x0E000004) return (PakFileType.SkillTable, null);
                else if (objectId == 0x0E000007) return (PakFileType.ChatPoseTable, "cps");
                else if (objectId == 0x0E00000D) return (PakFileType.ObjectHierarchy, "hrc");
                else if (objectId == 0x0E00000E) return (PakFileType.SpellTable, "cps");
                else if (objectId == 0x0E00000F) return (PakFileType.SpellComponentTable, "cps");
                else if (objectId == 0x0E000018) return (PakFileType.XpTable, "cps");
                else if (objectId == 0xE00001A) return (PakFileType.BadData, "bad");
                else if (objectId == 0x0E00001D) return (PakFileType.ContractTable, null);
                else if (objectId == 0x0E00001E) return (PakFileType.TabooTable, "taboo");
                else if (objectId == 0x0E00001F) return (PakFileType.FileToId, null);
                else if (objectId == 0x0E000020) return (PakFileType.NameFilterTable, "nft");
            }
            if (pakType == PakType.Language)
                switch (objectId >> 24)
                {
                    case 0x21: return (PakFileType.UILayout, null);
                    case 0x23: return (PakFileType.StringTable, null);
                    case 0x41: return (PakFileType.StringState, null);
                }
            Console.WriteLine($"Unknown file type: {objectId:X8}");
            return (0, null);
        }

        static string TextureExtensionLookup(FileMetadata source, BinaryReader r)
        {
            r.Seek(source.Position);
            r.Skip(16);
            var format = (SurfacePixelFormat)r.ReadUInt32();
            return format switch
            {
                SurfacePixelFormat.PFID_CUSTOM_RAW_JPEG => "jpg",
                SurfacePixelFormat.PFID_DXT1 => "dds",
                SurfacePixelFormat.PFID_DXT3 => "dds",
                SurfacePixelFormat.PFID_DXT5 => "dds",
                _ => "img",
            };
        }

        static string DbPropertyExtensionLookup(FileMetadata source, BinaryReader r)
        {
            return 0 switch
            {
                0 => "dbpc",
                1 => "pmat",
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}