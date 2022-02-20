using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Tes.Formats
{
    /// <summary>
    /// FormatExtensions
    /// </summary>
    public static class FormatExtensions
    {
        // object factory
        internal static Func<BinaryReader, FileMetadata, Task<object>> GetObjectFactory(this FileMetadata source)
        {
            Task<object> NiFactory(BinaryReader r, FileMetadata f) { var file = new NiFile(Path.GetFileNameWithoutExtension(f.Path)); file.Read(r); return Task.FromResult((object)file); }
            return Path.GetExtension(source.Path).ToLowerInvariant() switch
            {
                ".dds" => BinaryDds.Factory,
                ".nif" => NiFactory,
                _ => null,
            };
        }
    }
}