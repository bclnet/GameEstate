using GameEstate.Formats;
using OpenStack.Graphics;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Unreal.Formats
{
    /// <summary>
    /// FormatExtensions
    /// </summary>
    public static class FormatExtensions
    {
        // object factory
        internal static Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>> GetObjectFactoryFactory(this FileMetadata source)
        {
            return Path.GetExtension(source.Path).ToLowerInvariant() switch
            {
                var x when x == ".cfg" || x == ".txt" => BinaryText.Factory,
                ".dds" => BinaryDds.Factory,
                _ => null,
            };
        }
    }
}