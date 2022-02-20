using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Formats
{
    [DebuggerDisplay("{Path}")]
    public class FileMetadata
    {
        public int Id;
        public string Path;
        public int Compressed;
        public bool Crypted;
        public long PackedSize;
        public long FileSize;
        public long Position;
        public long Digest;
        // extra
        public byte[] Extra;
        public object FileInfo;
        public BinaryPakFile Pak;
        public object Tag;
        // factory
        public Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>> ObjectFactory;
    }
}