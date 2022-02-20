using GameEstate.Formats;
using OpenStack.Graphics;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Rsi.Formats
{
    /// <summary>
    /// FormatExtensions
    /// </summary>
    public static class FormatExtensions
    {
        // object factory
        internal static Func<BinaryReader, FileMetadata, Task<object>> GetObjectFactory(this FileMetadata source)
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
            return Path.GetExtension(source.Path).ToLowerInvariant() switch
            {
                ".dds" => DdsFactory,
                ".dcb" => DatabaseFactory,
                _ => null,
            };
        }
    }
}