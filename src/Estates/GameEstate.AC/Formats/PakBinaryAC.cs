using GameEstate.AC.Formats.FileTypes;
using GameEstate.Formats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Environment = GameEstate.AC.Formats.FileTypes.Environment;

namespace GameEstate.AC.Formats
{
    public class PakBinaryAC : PakBinary
    {
        public static readonly PakBinary Instance = new PakBinaryAC();
        PakBinaryAC() { }

        #region Headers

        const uint DAT_HEADER_OFFSET = 0x140;

        [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
        unsafe struct Header
        {
            public uint FileType;
            public uint BlockSize;
            public uint FileSize;
            [MarshalAs(UnmanagedType.U4)] public PakType DataSet;
            public uint DataSubset;

            public uint FreeHead;
            public uint FreeTail;
            public uint FreeCount;
            public uint BTree;

            public uint NewLRU;
            public uint OldLRU;
            public uint UseLRU; // UseLRU != 0

            public uint MasterMapID;

            public uint EnginePackVersion;
            public uint GamePackVersion;

            public fixed byte VersionMajor[16];
            public uint VersionMinor;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
        unsafe struct DirectoryHeader
        {
            public const int SizeOf = (sizeof(uint) * 0x3E) + sizeof(uint) + (File.SizeOf * 0x3D);
            public fixed uint Branches[0x3E];
            public uint EntryCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3D)] public File[] Entries;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
        unsafe struct File
        {
            public const int SizeOf = sizeof(uint) * 6;
            public uint BitFlags; // not-used
            public uint ObjectId;
            public uint FileOffset;
            public uint FileSize;
            public uint Date; // not-used
            public uint Iteration; // not-used
        }

        class Directory
        {
            public readonly DirectoryHeader Header;
            public readonly List<Directory> Directories = new List<Directory>();

            public unsafe Directory(BinaryReader r, long offset, int blockSize)
            {
                Header = ReadT<DirectoryHeader>(r, offset, DirectoryHeader.SizeOf, blockSize);
                if (Header.Branches[0] != 0)
                    for (var i = 0; i < Header.EntryCount + 1; i++)
                        Directories.Add(new Directory(r, Header.Branches[i], blockSize));
            }

            public void AddFiles(PakType pakType, IList<FileMetadata> files, string path, int blockSize)
            {
                //var did = 0;
                //Directories.ForEach(d => d.AddFiles(pakType, files, Path.Combine(path, did++.ToString())));
                Directories.ForEach(d => d.AddFiles(pakType, files, path, blockSize));
                for (var i = 0; i < Header.EntryCount; i++)
                {
                    var entry = Header.Entries[i];
                    var fileName = PakFileTypeHelper.GetFileName(entry.ObjectId, pakType, 0, out var type);
                    files.Add(new FileMetadata
                    {
                        Path = Path.Combine(path, fileName),
                        ObjectFactory = ObjectFactory(pakType, type),
                        Position = entry.FileOffset,
                        FileSize = entry.FileSize,
                        Digest = blockSize,
                        Tag = entry,
                    });
                }
            }
        }

        #endregion

        // object factory
        static Func<BinaryReader, FileMetadata, Task<object>> ObjectFactory(PakType pakType, PakFileType? type)
        {
            if (type == null)
                return null;
            switch (type.Value)
            {
                case PakFileType.LandBlock: return (r, m) => Task.FromResult((object)new Landblock(r));
                case PakFileType.LandBlockInfo: return (r, m) => Task.FromResult((object)new LandblockInfo(r));
                case PakFileType.EnvCell: return (r, m) => Task.FromResult((object)new EnvCell(r));
                //case PakFileType.LandBlockObjects: return null;
                //case PakFileType.Instantiation: return null;
                case PakFileType.GraphicsObject: return (r, m) => Task.FromResult((object)new GfxObj(r));
                case PakFileType.Setup: return (r, m) => Task.FromResult((object)new SetupModel(r));
                case PakFileType.Animation: return (r, m) => Task.FromResult((object)new Animation(r));
                //case PakFileType.AnimationHook: return null;
                case PakFileType.Palette: return (r, m) => Task.FromResult((object)new Palette(r));
                case PakFileType.SurfaceTexture: return (r, m) => Task.FromResult((object)new SurfaceTexture(r));
                case PakFileType.Texture: return (r, m) => Task.FromResult((object)new Texture(r));
                case PakFileType.Surface: return (r, m) => Task.FromResult((object)new Surface(r));
                case PakFileType.MotionTable: return (r, m) => Task.FromResult((object)new MotionTable(r));
                case PakFileType.Wave: return (r, m) => Task.FromResult((object)new Wave(r));
                case PakFileType.Environment: return (r, m) => Task.FromResult((object)new Environment(r));
                case PakFileType.ChatPoseTable: return (r, m) => Task.FromResult((object)new ChatPoseTable(r));
                case PakFileType.ObjectHierarchy: return (r, m) => Task.FromResult((object)new GeneratorTable(r)); //: Name wayoff
                case PakFileType.BadData: return (r, m) => Task.FromResult((object)new BadData(r));
                case PakFileType.TabooTable: return (r, m) => Task.FromResult((object)new TabooTable(r));
                case PakFileType.FileToId: return null;
                case PakFileType.NameFilterTable: return (r, m) => Task.FromResult((object)new NameFilterTable(r));
                case PakFileType.MonitoredProperties: return null;
                case PakFileType.PaletteSet: return (r, m) => Task.FromResult((object)new PaletteSet(r));
                case PakFileType.Clothing: return (r, m) => Task.FromResult((object)new ClothingTable(r));
                case PakFileType.DegradeInfo: return (r, m) => Task.FromResult((object)new GfxObjDegradeInfo(r));
                case PakFileType.Scene: return (r, m) => Task.FromResult((object)new Scene(r));
                case PakFileType.Region: return (r, m) => Task.FromResult((object)new RegionDesc(r));
                case PakFileType.KeyMap: return null;
                case PakFileType.RenderTexture: return (r, m) => Task.FromResult((object)new RenderTexture(r));
                case PakFileType.RenderMaterial: return null;
                case PakFileType.MaterialModifier: return null;
                case PakFileType.MaterialInstance: return null;
                case PakFileType.SoundTable: return (r, m) => Task.FromResult((object)new SoundTable(r));
                case PakFileType.UILayout: return null;
                case PakFileType.EnumMapper: return (r, m) => Task.FromResult((object)new EnumMapper(r));
                case PakFileType.StringTable: return (r, m) => Task.FromResult((object)new StringTable(r));
                case PakFileType.DidMapper: return (r, m) => Task.FromResult((object)new DidMapper(r));
                case PakFileType.ActionMap: return null;
                case PakFileType.DualDidMapper: return (r, m) => Task.FromResult((object)new DualDidMapper(r));
                case PakFileType.String: return (r, m) => Task.FromResult((object)new LanguageString(r)); //: Name wayoff
                case PakFileType.ParticleEmitter: return (r, m) => Task.FromResult((object)new ParticleEmitterInfo(r));
                case PakFileType.PhysicsScript: return (r, m) => Task.FromResult((object)new PhysicsScript(r));
                case PakFileType.PhysicsScriptTable: return (r, m) => Task.FromResult((object)new PhysicsScriptTable(r));
                case PakFileType.MasterProperty: return null;
                case PakFileType.Font: return (r, m) => Task.FromResult((object)new Font(r));
                case PakFileType.FontLocal: return null;
                case PakFileType.StringState: return (r, m) => Task.FromResult((object)new LanguageInfo(r)); //: Name wayoff
                case PakFileType.DbProperties: return null;
                case PakFileType.RenderMesh: return null;
                case PakFileType.WeenieDefaults: return null;
                case PakFileType.CharacterGenerator: return (r, m) => Task.FromResult((object)new CharGen(r));
                case PakFileType.SecondaryAttributeTable: return (r, m) => Task.FromResult((object)new SecondaryAttributeTable(r));
                case PakFileType.SkillTable: return (r, m) => Task.FromResult((object)new SkillTable(r));
                case PakFileType.SpellTable: return (r, m) => Task.FromResult((object)new SpellTable(r));
                case PakFileType.SpellComponentTable: return (r, m) => Task.FromResult((object)new SpellComponentTable(r));
                case PakFileType.TreasureTable: return null;
                case PakFileType.CraftTable: return null;
                case PakFileType.XpTable: return (r, m) => Task.FromResult((object)new XpTable(r));
                case PakFileType.Quests: return null;
                case PakFileType.GameEventTable: return null;
                case PakFileType.QualityFilter: return (r, m) => Task.FromResult((object)new QualityFilter(r));
                case PakFileType.CombatTable: return (r, m) => Task.FromResult((object)new CombatManeuverTable(r));
                case PakFileType.ItemMutation: return null;
                case PakFileType.ContractTable: return (r, m) => Task.FromResult((object)new ContractTable(r));
                default: return null;
                    //Iteration
            }
        }

        public unsafe override Task ReadAsync(BinaryPakFile source, BinaryReader r, ReadStage stage)
        {
            if (!(source is BinaryPakManyFile multiSource))
                throw new NotSupportedException();
            if (stage != ReadStage.File)
                throw new ArgumentOutOfRangeException(nameof(stage), stage.ToString());

            var files = multiSource.Files = new List<FileMetadata>();
            r.Position(DAT_HEADER_OFFSET);
            var header = r.ReadT<Header>(sizeof(Header));
            var directory = new Directory(r, header.BTree, (int)header.BlockSize);
            directory.AddFiles(header.DataSet, files, string.Empty, (int)header.BlockSize);
            return Task.CompletedTask;
        }

        public override Task<Stream> ReadDataAsync(BinaryPakFile source, BinaryReader r, FileMetadata file, Action<FileMetadata, string> exception = null) =>
            Task.FromResult((Stream)new MemoryStream(ReadBytes(r, file.Position, (int)file.FileSize, (int)file.Digest)));

        static T ReadT<T>(BinaryReader r, long offset, int size, int blockSize) => UnsafeX.MarshalT<T>(ReadBytes(r, offset, size, blockSize));

        static byte[] ReadBytes(BinaryReader r, long offset, int size, int blockSize)
        {
            r.Position(offset);
            var nextAddress = (long)r.ReadUInt32();
            var buffer = new byte[size];
            var bufferOffset = 0;
            int read;
            while (size > 0)
                if (size >= blockSize)
                {
                    read = r.Read(buffer, bufferOffset, blockSize - 4);
                    bufferOffset += read;
                    size -= read;
                    r.Position(nextAddress); nextAddress = r.ReadUInt32();
                }
                else
                {
                    r.Read(buffer, bufferOffset, size);
                    return buffer;
                }
            return buffer;
        }
    }
}