using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Aurora.Formats
{
    /// <summary>
    /// FormatExtensions
    /// </summary>
    public static class FormatExtensions
    {
        // object factory
        internal static Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>> GetObjectFactoryFactory(this FileMetadata source)
            => Path.GetExtension(source.Path).ToLowerInvariant() switch
            {
                ".dds" => BinaryDds.Factory,
                var x when x == ".dlg" || x == ".qdb" || x == ".qst" => BinaryGff.Factory,
                _ => null,
            };
    }
}