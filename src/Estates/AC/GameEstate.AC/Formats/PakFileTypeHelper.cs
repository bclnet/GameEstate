using System;

namespace GameEstate.AC.Formats
{
    public static class PakFileTypeHelper
    {
        struct Element
        {
            public PakFileType FileType;
            public PakTypeAttribute Type;
            public PakFileExtensionAttribute FileExtension;
            public PakFileIdRangeAttribute FileIdRange;
        }

        //static Dictionary<uint, (PakFileType type, object ext)> L00 = new Dictionary<uint, (PakFileType type, object ext)>();
        //static Dictionary<uint, (PakFileType type, object ext)> L25 = new Dictionary<uint, (PakFileType type, object ext)>();
        //static Dictionary<uint, (PakFileType type, object ext)> L24 = new Dictionary<uint, (PakFileType type, object ext)>();
        //static Dictionary<uint, (PakFileType type, object ext)> L16 = new Dictionary<uint, (PakFileType type, object ext)>();
        //static Dictionary<uint, (PakFileType type, object ext)> L12 = new Dictionary<uint, (PakFileType type, object ext)>();
        //DatFileType? fileType;
        //static List<DatFileType> EnumTypes = Enum.GetValues(typeof(DatFileType)).Cast<DatFileType>().ToList();
        //public PakFileType? GetFileType(DatDatabaseType datDatabaseType)
        //{
        //    if (fileType != null) return fileType.Value;
        //    var type = typeof(DatFileType);
        //    foreach (var enumType in EnumTypes)
        //    {
        //        var memInfo = type.GetMember(enumType.ToString());
        //        var datType = memInfo[0].GetCustomAttributes(typeof(DatDatabaseTypeAttribute), false).Cast<DatDatabaseTypeAttribute>().ToList();
        //        if (datType?.Count > 0 && datType[0].Type == datDatabaseType)
        //        {
        //            // file type matches, now check id range
        //            var idRange = memInfo[0].GetCustomAttributes(typeof(DatFileTypeIdRangeAttribute), false).Cast<DatFileTypeIdRangeAttribute>().ToList();
        //            if (idRange?.Count > 0 && idRange[0].BeginRange <= ObjectId && idRange[0].EndRange >= ObjectId) {                          fileType = enumType; break; } // id range matches
        //        }
        //    }
        //    return fileType;
        //}
        //static PakFileTypeHelper()
        //{
        //    var lookups = typeof(PakFileType).GetFields().Where(x => !x.IsSpecialName).Select(x => new Element
        //    {
        //        FileType = (PakFileType)x.GetRawConstantValue(),
        //        Type = (PakTypeAttribute)x.GetCustomAttributes(typeof(PakTypeAttribute), false).FirstOrDefault(),
        //        FileExtension = (PakFileExtensionAttribute)x.GetCustomAttributes(typeof(PakFileExtensionAttribute), false).FirstOrDefault(),
        //        FileIdRange = (PakFileIdRangeAttribute)x.GetCustomAttributes(typeof(PakFileIdRangeAttribute), false).FirstOrDefault(),
        //    }).Where(x => x.FileIdRange != null).ToLookup(x => x.FileIdRange.End - x.FileIdRange.Begin + 1);
        //    L00 = lookups[1].ToDictionary(x => x.FileIdRange.Begin, x => (x.FileType, x.FileExtension?.Value));
        //    L25 = lookups[1 << 25].ToDictionary(x => x.FileIdRange.Begin >> 25, x => (x.FileType, x.FileExtension?.Value));
        //    L24 = lookups[1 << 24].ToDictionary(x => x.FileIdRange.Begin >> 24, x => (x.FileType, x.FileExtension?.Value));
        //    L16 = lookups[1 << 16].ToDictionary(x => x.FileIdRange.Begin >> 16, x => (x.FileType, x.FileExtension?.Value));
        //    L12 = lookups[1 << 12].ToDictionary(x => x.FileIdRange.Begin >> 12, x => (x.FileType, x.FileExtension?.Value));
        //}
        //public static (PakFileType type, object ext)? GetFileType2(uint objectId, PakType pakType)
        //{
        //    if (pakType == PakType.Cell)
        //    {
        //        if ((objectId & 0xFFFF) == 0xFFFF) return (PakFileType.LandBlock, "land");
        //        else if ((objectId & 0xFFFF) == 0xFFFE) return (PakFileType.LandBlockInfo, "lbi");
        //        return (PakFileType.EnvCell, "cell");
        //    }
        //    else if (objectId >= 0x78000000 && objectId <= 0x7FFFFFFF) return (PakFileType.DbProperties, DbPropertyExtensionLookup(0));
        //    else if (objectId >= 0x40001000 && objectId <= 0x400FFFFF) return (PakFileType.FontLocal, "font_local");
        //    else if (L00.TryGetValue(objectId, out var value)) return value;
        //    else if (L25.TryGetValue(objectId >> 25, out value)) return value;
        //    else if (L24.TryGetValue(objectId >> 24, out value)) return value;
        //    else if (L16.TryGetValue(objectId >> 16, out value)) return value;
        //    else if (L12.TryGetValue(objectId >> 12, out value)) return value;
        //    Console.WriteLine($"Unknown file type: {objectId:X8}");
        //    return null;
        //}

        public static string TextureExtensionLookup(uint x)
            => x switch
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

        public static string DbPropertyExtensionLookup(uint x)
            => x switch
            {
                0 => "dbpc",
                1 => "pmat",
                _ => throw new ArgumentOutOfRangeException(nameof(x)),
            };

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

        public static (PakFileType type, object ext)? GetFileType(uint objectId, PakType pakType)
        {
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
                    case 0x06: return (PakFileType.Texture, new PakFileExtensionAttribute(typeof(PakFileTypeHelper), "TextureExtensionLookup").Value);
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
                    case 0x78: return (PakFileType.DbProperties, new PakFileExtensionAttribute(typeof(PakFileTypeHelper), "DbPropertyExtensionLookup").Value);
                }
                switch (objectId >> 16)
                {
                    case 0x0E01: return (PakFileType.QualityFilter, "");
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
            {
                switch (objectId >> 24)
                {
                    case 0x21: return (PakFileType.UILayout, "");
                    case 0x23: return (PakFileType.StringTable, "");
                    case 0x41: return (PakFileType.StringState, "");
                }
            }
            Console.WriteLine($"Unknown file type: {objectId:X8}");
            return null;
        }
    }
}
