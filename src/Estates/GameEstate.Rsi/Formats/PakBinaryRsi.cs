// https://github.com/dolkensp/unp4k/releases
using GameEstate.Formats;
using OpenStack.Graphics;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Rsi.Formats
{
    /// <summary>
    /// PakBinaryRsi
    /// </summary>
    /// <seealso cref="GameEstate.Formats._Packages.PakBinaryZip" />
    public class PakBinaryRsi : PakBinaryZip
    {
        public new static readonly PakBinary Instance = new PakBinaryRsi();
        PakBinaryRsi() : base(ObjectFactory, P4kKey) { }

        static readonly byte[] P4kKey = new byte[] { 0x5E, 0x7A, 0x20, 0x02, 0x30, 0x2E, 0xEB, 0x1A, 0x3B, 0xB6, 0x17, 0xC3, 0x0F, 0xDE, 0x1E, 0x47 };

        // object factory
        static Func<BinaryReader, FileMetadata, Task<object>> ObjectFactory(string path)
        {
            Task<object> DdsFactory(BinaryReader r, FileMetadata f)
            {
                var tex = new TextureInfo();
                tex.ReadDds(r);
                return Task.FromResult((object)tex);
            }
            Task<object> DatabaseFactory(BinaryReader r, FileMetadata f)
            {
                var file = new DatabaseFile();
                file.Read(r);
                return Task.FromResult((object)file);
            }
            return Path.GetExtension(path).ToLowerInvariant() switch
            {
                ".dds" => DdsFactory,
                ".dcb" => DatabaseFactory,
                _ => null,
            };
        }
    }
}