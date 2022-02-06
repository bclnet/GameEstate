using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameEstate.AC.Formats
{
    #region Attributes

    [AttributeUsage(AttributeTargets.Field)]
    public class PakTypeAttribute : Attribute
    {
        public PakTypeAttribute(PakType type) => Type = type;
        public PakType Type { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PakFileTypeAttribute : Attribute
    {
        public PakFileTypeAttribute(PakFileType type) => Type = type;
        public PakFileType Type { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class PakFileExtensionAttribute : Attribute
    {
        public PakFileExtensionAttribute(string extension) => Value = extension;
        public PakFileExtensionAttribute(Type classType, string methodName)
        {
            ExtensionMethod = classType.GetMethod(methodName);
            Value = (Func<uint, string>)(x => (string)ExtensionMethod.Invoke(null, new object[] { x }));
        }
        public object Value { get; set; }
        MethodInfo ExtensionMethod { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class PakFileIdRangeAttribute : Attribute
    {
        public PakFileIdRangeAttribute(uint begin, uint end) { Begin = begin; End = end; }
        public uint Begin { get; set; }
        public uint End { get; set; }
    }

    public enum PakType : uint
    {
        Portal = 1, // client_portal.dat and client_highres.dat both have this value
        Cell = 2, // client_cell_1.dat
        Language = 3 // client_local_English.dat
    }

    public enum PakFileType : uint
    {
        /// <summary>
        /// File Format:
        ///     DWORD LandblockId
        ///     DWORD LandblockInfoId
        ///     WORD x 81 = Terrain Map
        ///     WORD x 81 = Height Map
        /// </summary>
        [PakType(PakType.Cell)] LandBlock = 1, // DB_TYPE_LANDBLOCK
        /// <summary>
        /// File Format:
        ///     DWORD LandblockId
        ///     DWORD Number of Cells
        ///     DWORD Number of static objects (numObj)
        ///         numObj of:
        ///             DWORD Static Object id
        ///             POSITION Object Position
        ///     WORD Building Count (numBldg)
        ///     WORD Building Flags
        ///         numBldg of:
        ///             DWORD MeshId
        ///             POSITION Building Position
        ///     DWORD unknown
        ///     DWORD numPortals
        ///         numPortals of:
        ///             WORD Flags
        ///             WORD CellID
        ///             WORD Unknown
        ///             WORD numCells
        ///                 numCells of:
        ///                     WORD CellID
        ///             
        /// POSITION ::
        ///     FLOAT Vector.X
        ///     FLOAT Vector.Y
        ///     FLOAT Vector.Z
        ///     FLOAT Quat.W
        ///     FLOAT Quat.X
        ///     FLOAT Quat.Y
        ///     FLOAT Quat.Z
        /// </summary>
        [PakType(PakType.Cell), PakFileExtension("lbi")] LandBlockInfo = 2, // DB_TYPE_LBI
        [PakType(PakType.Cell), PakFileIdRange(0x01010000, 0x013EFFFF)] EnvCell = 3, // DB_TYPE_ENVCELL
        /// <summary>
        /// usage of this is currently unknown.  exists in the client, but has no discernable
        /// source dat file.  appears to be a server file not distributed to clients.
        /// </summary>
        [PakFileExtension("lbo")] LandBlockObjects = 4, // DB_TYPE_LBO
        /// <summary>
        /// usage of this is currently unknown.  exists in the client, but has no discernable
        /// source dat file.  appears to be a server file not distributed to clients.
        /// </summary>
        [PakFileExtension("ins")] Instantiation = 5, // DB_TYPE_INSTANTIATION
        [PakType(PakType.Portal), PakFileExtension("obj"), PakFileIdRange(0x01000000, 0x0100FFFF)] GraphicsObject = 6, // DB_TYPE_GFXOBJ
        [PakType(PakType.Portal), PakFileExtension("set"), PakFileIdRange(0x02000000, 0x0200FFFF)] Setup = 7, // DB_TYPE_SETUP
        [PakType(PakType.Portal), PakFileExtension("anm"), PakFileIdRange(0x03000000, 0x0300FFFF)] Animation = 8, // DB_TYPE_ANIM
        /// <summary>
        /// usage of this is currently unknown.  exists in the client, but has no discernable
        /// source dat file.  appears to be a server file not distributed to clients.
        /// </summary>
        [PakFileExtension("hk")] AnimationHook = 9, // DB_TYPE_ANIMATION_HOOK
        [PakType(PakType.Portal), PakFileExtension("pal"), PakFileIdRange(0x04000000, 0x0400FFFF)] Palette = 10, // DB_TYPE_PALETTE
        [PakType(PakType.Portal), PakFileExtension("texture"), PakFileIdRange(0x05000000, 0x05FFFFFF)] SurfaceTexture = 11, // DB_TYPE_SURFACETEXTURE
        /// <summary>
        /// the 5th dword of these files has values from the following enum:
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/bb172558(v=vs.85).aspx
        /// plus the additional values:
        ///     0x000000F4 - Same as 0x00000001C / D3DFMT_A8
        ///     0x000001F4 - JPEG
        ///     
        /// all the files contain a 6-DWORD header (offset indices):
        ///     0: objectId
        ///     4: unknown
        ///     8: width
        ///     12: height
        ///     16: format (see above)
        ///     20: length
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension(typeof(PakFileTypeHelper), "TextureExtensionLookup"), PakFileIdRange(0x06000000, 0x07FFFFFF)] Texture = 12, // DB_TYPE_RENDERSURFACE
        /// <summary>
        /// indexed in client as "materials" for some reason
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("surface"), PakFileIdRange(0x08000000, 0x0800FFFF)] Surface = 13, // DB_TYPE_SURFACE
        [PakType(PakType.Portal), PakFileExtension("dsc"), PakFileIdRange(0x09000000, 0x0900FFFF)] MotionTable = 14, // DB_TYPE_MTABLE
        /// <summary>
        /// indexed as "sound" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("wav"), PakFileIdRange(0x0A000000, 0x0A00FFFF)] Wave = 15, // DB_TYPE_WAVE
        /// <summary>
        /// File content structure:
        /// 0: DWORD LandblockId
        /// 4: DWORD CellCount
        /// 8: CellCount blocks of Cells
        /// 
        /// Cell content:
        /// 0: DWORD Cell Index
        /// 4: DWORD Polygon Count (List1)
        /// 8: DWORD Polygon Count (List2)
        /// 12: DWORD Polygon Pointer Count (???)
        /// 16: Vertex Array
        /// ??: List1 Polygons
        /// 
        /// Vertex Array content:
        /// 0: DWORD Vertex Type
        /// 4: DWORD Vertex Count
        /// 8: VertexCount block of Vertecis (whole thing aligned to 32-byte boundary
        /// 
        /// Vertex Content:
        /// 0: WORD Vertex Index
        /// 2: WORD Count
        /// 4: FLOAT Origin X
        /// 8: FLOAT Origin Y
        /// 12: FLOAT Origin Z
        /// 16: FLOAT Normal X
        /// 20: FLOAT Normal Y
        /// 24: FLOAT Normal Z
        /// 28: VertexUV Array of Vertex.Count length
        ///     0: FLOAT Vertex U
        ///     4: FLOAT Vertex V
        /// 
        /// Polygon Content:
        /// 0: WORD Polygon Index
        /// 2: BYTE Vertex Count
        /// 3: BYTE Poly Type
        /// 4: DWORD Cull Mode - https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.graphics.cullmode(v=xnagamestudio.31).aspx
        ///     NOTE: AC doesn't seem to fully respect these Cull mode values.  1 appears to be None instead of CCW, and 2 is None.
        /// 8: WORD Front Texture Index
        /// 10: WORD Back Texture Index
        /// 12: VertexCount of:
        ///     0: WORD Vertex Index in previous Vertex Content
        /// if ((PolyType & 4) == 0)
        ///     VertexCount of:
        ///     BYTE Front-Face Vertex Index
        /// if ((PolyType & 8) == 0 && CullMode == 2)
        ///     VertexCount of:
        ///     BYTE Back-Face Vertex Index
        /// 
        /// Note: If CullMode is 1, copy Front-Face data to Back-face
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("env"), PakFileIdRange(0x0D000000, 0x0D00FFFF)] Environment = 16, // DB_TYPE_ENVIRONMENT
        /// <summary>
        /// indexed as "ui" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("cps"), PakFileIdRange(0x0E000007, 0x0E000007)] ChatPoseTable = 17, // DB_TYPE_CHAT_POSE_TABLE
        /// <summary>
        /// indexed as "DungeonCfgs" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("hrc"), PakFileIdRange(0x0E00000D, 0x0E00000D)] ObjectHierarchy = 18, // DB_TYPE_OBJECT_HIERARCHY
        /// <summary>
        /// indexed as "weenie" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("bad"), PakFileIdRange(0x0E00001A, 0x0E00001A)] BadData = 19, // DB_TYPE_BADDATA
        /// <summary>
        /// indexed as "weenie" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("taboo"), PakFileIdRange(0x0E00001E, 0x0E00001E)] TabooTable = 20, // DB_TYPE_TABOO_TABLE
        [PakType(PakType.Portal), PakFileIdRange(0x0E00001F, 0x0E00001F)] FileToId = 21, // DB_TYPE_FILE2ID_TABLE
        /// <summary>
        /// indexed as "namefilter" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("nft"), PakFileIdRange(0x0E000020, 0x0E000020)] NameFilterTable = 22, // DB_TYPE_NAME_FILTER_TABLE
        /// <summary>
        /// indexed as "properties" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("monprop"), PakFileIdRange(0x0E020000, 0x0E02FFFF)] MonitoredProperties = 23, // DB_TYPE_MONITOREDPROPERTIES
        [PakType(PakType.Portal), PakFileExtension("pst"), PakFileIdRange(0x0F000000, 0x0F00FFFF)] PaletteSet = 24, // DB_TYPE_PAL_SET
        [PakType(PakType.Portal), PakFileExtension("clo"), PakFileIdRange(0x10000000, 0x1000FFFF)] Clothing = 25, // DB_TYPE_CLOTHING
        [PakType(PakType.Portal), PakFileExtension("deg"), PakFileIdRange(0x11000000, 0x1100FFFF)] DegradeInfo = 26, // DB_TYPE_DEGRADEINFO
        [PakType(PakType.Portal), PakFileExtension("scn"), PakFileIdRange(0x12000000, 0x1200FFFF)] Scene = 27, // DB_TYPE_SCENE 
        /// <summary>
        /// indexed as "landscape" by the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("rgn"), PakFileIdRange(0x13000000, 0x1300FFFF)] Region = 28, // DB_TYPE_REGION
        [PakType(PakType.Portal), PakFileExtension("keymap"), PakFileIdRange(0x14000000, 0x1400FFFF)] KeyMap = 29, // DB_TYPE_KEYMAP
        /// <summary>
        /// indexed as "textures" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("rtexture"), PakFileIdRange(0x15000000, 0x15FFFFFF)] RenderTexture = 30, // DB_TYPE_RENDERTEXTURE 
        /// <summary>
        /// indexed as "materials" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("mat"), PakFileIdRange(0x16000000, 0x16FFFFFF)] RenderMaterial = 31, // DB_TYPE_RENDERMATERIAL 
        /// <summary>
        /// indexed as "materials" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("mm"), PakFileIdRange(0x17000000, 0x17FFFFFF)] MaterialModifier = 32, // DB_TYPE_MATERIALMODIFIER 
        /// <summary>
        /// indexed as "materials" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("mi"), PakFileIdRange(0x18000000, 0x18FFFFFF)] MaterialInstance = 33, // DB_TYPE_MATERIALINSTANCE
        /// <summary>
        /// SoundTable
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("stb"), PakFileIdRange(0x20000000, 0x2000FFFF)] SoundTable = 34, // DB_TYPE_STABLE
        /// <summary>
        /// This is in the Language dat (client_local_English.dat)
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("uil"), PakFileIdRange(0x21000000, 0x21FFFFFF)] UILayout = 35, // DB_TYPE_UI_LAYOUT
        /// <summary>
        /// indexed as "emp" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("emp"), PakFileIdRange(0x22000000, 0x22FFFFFF)] EnumMapper = 36, // DB_TYPE_ENUM_MAPPER
        /// <summary>
        /// This is in the Language dat (client_local_English.dat)
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("stt"), PakFileIdRange(0x23000000, 0x24FFFFFF)] StringTable = 37, // DB_TYPE_STRING_TABLE 
        /// <summary>
        /// indexed as "emp/idmap" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("imp"), PakFileIdRange(0x25000000, 0x25FFFFFF)] DidMapper = 38, // DB_TYPE_DID_MAPPER 
        [PakType(PakType.Portal), PakFileExtension("actionmap"), PakFileIdRange(0x26000000, 0x2600FFFF)] ActionMap = 39, // DB_TYPE_ACTIONMAP 
        /// <summary>
        /// indexed as "emp/idmap" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("dimp"), PakFileIdRange(0x27000000, 0x27FFFFFF)] DualDidMapper = 40, // DB_TYPE_DUAL_DID_MAPPER
        [PakType(PakType.Portal), PakFileExtension("str"), PakFileIdRange(0x31000000, 0x3100FFFF)] String = 41, // DB_TYPE_STRING
        /// <summary>
        /// inedexed as "emt" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("emt"), PakFileIdRange(0x32000000, 0x3200FFFF)] ParticleEmitter = 42, // DB_TYPE_PARTICLE_EMITTER 
        /// <summary>
        /// inedexed as "pes" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("pes"), PakFileIdRange(0x33000000, 0x3300FFFF)] PhysicsScript = 43, // DB_TYPE_PHYSICS_SCRIPT 
        /// <summary>
        /// inedexed as "pet" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("pet"), PakFileIdRange(0x34000000, 0x3400FFFF)] PhysicsScriptTable = 44, // DB_TYPE_PHYSICS_SCRIPT_TABLE 
        /// <summary>
        /// inedexed as "emt/property" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("mpr"), PakFileIdRange(0x39000000, 0x39FFFFFF)] MasterProperty = 45, // DB_TYPE_MASTER_PROPERTY 
        [PakType(PakType.Portal), PakFileExtension("font"), PakFileIdRange(0x40000000, 0x40000FFF)] Font = 46, // DB_TYPE_FONT 
        [PakType(PakType.Portal), PakFileExtension("font_local"), PakFileIdRange(0x40001000, 0x400FFFFF)] FontLocal = 47, // DB_TYPE_FONT_LOCAL 
        /// <summary>
        /// This is located in the Language dat (client_local_English.dat)
        /// "stringtable" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("sti"), PakFileIdRange(0x41000000, 0x41FFFFFF)] StringState = 48, // DB_TYPE_STRING_STATE
        [PakType(PakType.Portal), PakFileExtension(typeof(PakFileTypeHelper), "DbPropertyExtensionLookup"), PakFileIdRange(0x78000000, 0x7FFFFFFF)] DbProperties = 49, // DB_TYPE_DBPROPERTIES
        /// <summary>
        /// indexed as "mesh" in the client
        /// </summary>
        [PakType(PakType.Portal), PakFileExtension("rendermesh"), PakFileIdRange(0x19000000, 0x19FFFFFF)] RenderMesh = 50, // DB_TYPE_RENDER_MESH
                                                                                                                       // the following special files are called out in a different section of the decompiled client:
        [PakType(PakType.Portal), PakFileIdRange(0x0E000001, 0x0E000001)] WeenieDefaults = 97, // DB_TYPE_WEENIE_DEF
        [PakType(PakType.Portal), PakFileIdRange(0x0E000002, 0x0E000002)] CharacterGenerator = 98, // DB_TYPE_CHAR_GEN_0
        [PakType(PakType.Portal), PakFileIdRange(0x0E000003, 0x0E000003)] SecondaryAttributeTable = 99, // DB_TYPE_ATTRIBUTE_2ND_TABLE_0
        [PakType(PakType.Portal), PakFileIdRange(0x0E000004, 0x0E000004)] SkillTable = 100, // DB_TYPE_SKILL_TABLE_0
        [PakType(PakType.Portal), PakFileIdRange(0x0E00000E, 0x0E00000E)] SpellTable = 101, // DB_TYPE_SPELL_TABLE_0
        [PakType(PakType.Portal), PakFileIdRange(0x0E00000F, 0x0E00000F)] SpellComponentTable = 102, // DB_TYPE_SPELLCOMPONENT_TABLE_0
        [PakType(PakType.Portal), PakFileIdRange(0x0E000010, 0x0E000010)] TreasureTable = 103, // DB_TYPE_W_TREASURE_SYSTEM //:SKY IdRangeError
        [PakType(PakType.Portal), PakFileIdRange(0x0E000019, 0x0E000019)] CraftTable = 104, // DB_TYPE_W_CRAFT_TABLE
        [PakType(PakType.Portal), PakFileIdRange(0x0E000018, 0x0E000018)] XpTable = 105, // DB_TYPE_XP_TABLE_0
        [PakType(PakType.Portal), PakFileIdRange(0x0E00001B, 0x0E00001B)] Quests = 106, // DB_TYPE_QUEST_DEF_DB_0
        [PakType(PakType.Portal), PakFileIdRange(0x0E00001C, 0x0E00001C)] GameEventTable = 107, // DB_TYPE_GAME_EVENT_DB
        [PakType(PakType.Portal), PakFileIdRange(0x0E010000, 0x0E01FFFF)] QualityFilter = 108, // DB_TYPE_QUALITY_FILTER_0
        [PakType(PakType.Portal), PakFileIdRange(0x30000000, 0x3000FFFF)] CombatTable = 109, // DB_TYPE_COMBAT_TABLE_0
        [PakType(PakType.Portal), PakFileIdRange(0x38000000, 0x3800FFFF)] ItemMutation = 110, // DB_TYPE_MUTATE_FILTER
        [PakType(PakType.Portal), PakFileIdRange(0x0E00001D, 0x0E00001D)] ContractTable = 111, // DB_TYPE_CONTRACT_TABLE_0
    }

    #endregion

    public static class PakFileTypeHelper
    {
        static Dictionary<uint, (PakFileType type, object ext)> L00 = new Dictionary<uint, (PakFileType type, object ext)>();
        static Dictionary<uint, (PakFileType type, object ext)> L25 = new Dictionary<uint, (PakFileType type, object ext)>();
        static Dictionary<uint, (PakFileType type, object ext)> L24 = new Dictionary<uint, (PakFileType type, object ext)>();
        static Dictionary<uint, (PakFileType type, object ext)> L16 = new Dictionary<uint, (PakFileType type, object ext)>();
        static Dictionary<uint, (PakFileType type, object ext)> L12 = new Dictionary<uint, (PakFileType type, object ext)>();

        struct Element
        {
            public PakFileType FileType;
            public PakTypeAttribute Type;
            public PakFileExtensionAttribute FileExtension;
            public PakFileIdRangeAttribute FileIdRange;
        }

        static PakFileTypeHelper()
        {
            var lookups = typeof(PakFileType).GetFields().Where(x => !x.IsSpecialName).Select(x => new Element
            {
                FileType = (PakFileType)x.GetRawConstantValue(),
                Type = (PakTypeAttribute)x.GetCustomAttributes(typeof(PakTypeAttribute), false).FirstOrDefault(),
                FileExtension = (PakFileExtensionAttribute)x.GetCustomAttributes(typeof(PakFileExtensionAttribute), false).FirstOrDefault(),
                FileIdRange = (PakFileIdRangeAttribute)x.GetCustomAttributes(typeof(PakFileIdRangeAttribute), false).FirstOrDefault(),
            }).Where(x => x.FileIdRange != null).ToLookup(x => x.FileIdRange.End - x.FileIdRange.Begin + 1);
            L00 = lookups[1].ToDictionary(x => x.FileIdRange.Begin, x => (x.FileType, x.FileExtension?.Value));
            L25 = lookups[1 << 25].ToDictionary(x => x.FileIdRange.Begin >> 25, x => (x.FileType, x.FileExtension?.Value));
            L24 = lookups[1 << 24].ToDictionary(x => x.FileIdRange.Begin >> 24, x => (x.FileType, x.FileExtension?.Value));
            L16 = lookups[1 << 16].ToDictionary(x => x.FileIdRange.Begin >> 16, x => (x.FileType, x.FileExtension?.Value));
            L12 = lookups[1 << 12].ToDictionary(x => x.FileIdRange.Begin >> 12, x => (x.FileType, x.FileExtension?.Value));
        }

        public static string TextureExtensionLookup(uint x)
        {
            return x switch
            {
                0 => "jpg",
                1 => "dds",
                2 => "tga",
                3 => "iff",
                4 => "256",
                5 => "csi",
                6 => "alp",
                _ => throw new ArgumentOutOfRangeException(nameof(x)),
            };
        }

        public static string DbPropertyExtensionLookup(uint x)
        {
            return x switch
            {
                0 => "dbpc",
                1 => "pmat",
                _ => throw new ArgumentOutOfRangeException(nameof(x)),
            };
        }

        public static string GetFileName(uint id, PakType dataType, uint value, out PakFileType? type)
        {
            var fileType = GetFileType(id, dataType);
            if (fileType == null) { type = null; return $"{id:X8}"; }
            type = fileType.Value.type;
            return fileType.Value.ext switch
            {
                null => $"{id:X8}",
                string ext => $"{id:X8}.{ext}",
                Func<uint, string> ext => $"{id:X8}.{ext(value)}",
                _ => throw new ArgumentOutOfRangeException(nameof(fileType.Value.ext)),
            };
        }

        public static (PakFileType type, object ext)? GetFileType(uint id, PakType pakType)
        {
            if (pakType == PakType.Cell)
            {
                if ((id & 0xFFFF) == 0xFFFF) return (PakFileType.LandBlock, "land");
                else if ((id & 0xFFFF) == 0xFFFE) return (PakFileType.LandBlockInfo, "lbi");
                return (PakFileType.EnvCell, "cell");
            }
            else if (id >= 0x78000000 && id <= 0x7FFFFFFF) return (PakFileType.DbProperties, DbPropertyExtensionLookup(0));
            else if (id >= 0x40001000 && id <= 0x400FFFFF) return (PakFileType.FontLocal, "font_local");
            else if (L00.TryGetValue(id, out var value)) return value;
            else if (L25.TryGetValue(id >> 25, out value)) return value;
            else if (L24.TryGetValue(id >> 24, out value)) return value;
            else if (L16.TryGetValue(id >> 16, out value)) return value;
            else if (L12.TryGetValue(id >> 12, out value)) return value;
            Console.WriteLine($"Unknown file type: {id:X8}");
            return null;
        }
    }
}

//switch (id >> 24)
//{
//    case 0x01: return FileType.GraphicsObject;
//    case 0x02: return FileType.Setup;
//    case 0x03: return FileType.Animation;
//    case 0x04: return FileType.Palette;
//    case 0x05: return FileType.SurfaceTexture;
//    case 0x06: return FileType.Texture;
//    case 0x08: return FileType.Surface;
//    case 0x09: return FileType.MotionTable;
//    case 0x0A: return FileType.Wave;
//    case 0x0D: return FileType.Environment;
//    case 0x0F: return FileType.PaletteSet;
//    case 0x10: return FileType.Clothing;
//    case 0x11: return FileType.DegradeInfo;
//    case 0x12: return FileType.Scene;
//    case 0x13: return FileType.Region;
//    case 0x20: return FileType.SoundTable;
//    case 0x22: return FileType.EnumMapper;
//    case 0x23: return FileType.StringTable;
//    case 0x25: return FileType.DidMapper;
//    case 0x27: return FileType.DualDidMapper;
//    case 0x30: return FileType.CombatTable;
//    case 0x32: return FileType.ParticleEmitter;
//    case 0x33: return FileType.PhysicsScript;
//    case 0x34: return FileType.PhysicsScriptTable;
//    case 0x40: return FileType.Font;
//}
//if (id == 0x0E000002) return FileType.CharacterGenerator;
//else if (id == 0x0E000007) return FileType.ChatPoseTable;
//else if (id == 0x0E00000D) return FileType.ObjectHierarchy;
//else if (id == 0xE00001A) return FileType.BadData;
//else if (id == 0x0E00001E) return FileType.TabooTable;
//else if (id == 0x0E00001F) return FileType.FileToId;
//else if (id == 0x0E000020) return FileType.NameFilterTable;
//else if (id == 0x0E020000) return FileType.MonitoredProperties;
//Console.WriteLine($"Unknown file type: {id:X8}");
//return null;