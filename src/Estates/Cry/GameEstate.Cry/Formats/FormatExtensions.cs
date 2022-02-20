using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Cry.Formats
{
    /// <summary>
    /// FormatExtensions
    /// </summary>
    public static class FormatExtensions
    {
        // object factory
        internal static Func<BinaryReader, FileMetadata, Task<object>> GetObjectFactory(this FileMetadata source)
        {
            switch (Path.GetExtension(source.Path).ToLowerInvariant())
            {
                case ".dds": return BinaryDds.Factory;
                case ".soc":
                case ".cgf":
                case ".cga":
                case ".chr":
                case ".skin":
                case ".anim": return (r, f) => CryFile.Factory(pak, r, f);
                default: return null;
            }
        }
    }
}