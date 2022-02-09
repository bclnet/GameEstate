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

        // was:DatDatabaseHeader
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

            public uint MasterMapId;

            public uint EnginePackVersion;
            public uint GamePackVersion;

            public fixed byte VersionMajor[16];
            public uint VersionMinor;
        }

        // was:DatDirectoryHeader
        [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
        unsafe struct DirectoryHeader
        {
            public const int SizeOf = (sizeof(uint) * 0x3E) + sizeof(uint) + (File.SizeOf * 0x3D);
            public fixed uint Branches[0x3E];
            public uint EntryCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3D)] public File[] Entries;
        }

        // was:DatFile
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

        // was:DatDirectory
        class Directory
        {
            public readonly DirectoryHeader Header;
            public readonly List<Directory> Directories = new List<Directory>();

            public unsafe Directory(BinaryReader r, long offset, int blockSize)
            {
                Header = ReadT<DirectoryHeader>(r, offset, DirectoryHeader.SizeOf, blockSize);
                if (Header.Branches[0] != 0) for (var i = 0; i < Header.EntryCount + 1; i++) Directories.Add(new Directory(r, Header.Branches[i], blockSize));
            }

            public void AddFiles(PakType pakType, IList<FileMetadata> files, string path, int blockSize)
            {
                //var did = 0; Directories.ForEach(d => d.AddFiles(pakType, files, Path.Combine(path, did++.ToString()), blockSize));
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
            if (type == null) return null;
            return type.Value switch
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

        public unsafe override Task ReadAsync(BinaryPakFile source, BinaryReader r, ReadStage stage)
        {
            if (!(source is BinaryPakManyFile multiSource)) throw new NotSupportedException();
            if (stage != ReadStage.File) throw new ArgumentOutOfRangeException(nameof(stage), stage.ToString());
            //if (ACPakManager.CellDat == null) ACPakManager.Initialize(source);

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
            int read;
            var buffer = new byte[size];
            r.Position(offset);
            // Dat "file" is broken up into sectors that are not neccessarily congruous. Next address is stored in first four bytes of each sector.
            var nextAddress = (long)r.ReadUInt32();
            var bufferOffset = 0;

            while (size > 0)
                if (size >= blockSize)
                {
                    read = r.Read(buffer, bufferOffset, blockSize - 4); // Read in our sector into the buffer[]
                    bufferOffset += read; // Adjust this so we know where in our buffer[] the next sector gets appended to
                    size -= read; // Decrease this by the amount of data we just read into buffer[] so we know how much more to go
                    r.Position(nextAddress); // Move the file pointer to the start of the next sector we read above.
                    nextAddress = r.ReadUInt32(); // Get the start location of the next sector.
                }
                else { r.Read(buffer, bufferOffset, size); return buffer; }
            return buffer;
        }
    }
}