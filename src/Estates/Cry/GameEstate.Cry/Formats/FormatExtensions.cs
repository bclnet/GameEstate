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
        internal static Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>> GetObjectFactory(this FileMetadata source)
            => Path.GetExtension(source.Path).ToLowerInvariant() switch
            {
                ".dds" => BinaryDds.Factory,
                var x when x == ".soc" || x == ".cgf" || x == ".cga" || x == ".chr" || x == ".skin" || x == ".anim" => CryFile.Factory,
                _ => null,
            };
    }
}

//