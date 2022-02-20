using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Cry.Formats
{
    public class PakBinaryCry : AbstractPakBinaryZip2
    {
        public static readonly PakBinary Instance = new PakBinaryCry();

        protected override Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>> GetObjectFactory(FileMetadata source) => source.GetObjectFactory();
    }
}