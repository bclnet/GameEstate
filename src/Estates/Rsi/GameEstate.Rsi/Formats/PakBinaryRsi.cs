// https://github.com/dolkensp/unp4k/releases
using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Rsi.Formats
{
    /// <summary>
    /// PakBinaryRsi
    /// </summary>
    /// <seealso cref="GameEstate.Formats._Packages.PakBinaryZip" />
    public class PakBinaryRsi : AbstractPakBinaryZip
    {
        static readonly byte[] P4kKey = new byte[] { 0x5E, 0x7A, 0x20, 0x02, 0x30, 0x2E, 0xEB, 0x1A, 0x3B, 0xB6, 0x17, 0xC3, 0x0F, 0xDE, 0x1E, 0x47 };
        public static readonly PakBinary Instance = new PakBinaryRsi();
        
        PakBinaryRsi() : base(P4kKey) { }

        protected override Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>> GetObjectFactory(FileMetadata source) => source.GetObjectFactory();
    }
}