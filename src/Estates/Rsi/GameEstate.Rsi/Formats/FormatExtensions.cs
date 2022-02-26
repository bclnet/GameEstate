using GameEstate.Cry.Formats;
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
        internal static Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>> GetObjectFactoryFactory(this FileMetadata source)
        {
            //Task<object> DdsFactory(BinaryReader r, FileMetadata f, EstatePakFile s)
            //{
            //    var tex = new TextureInfo();
            //    tex.ReadDds(r);
            //    return Task.FromResult((object)tex);
            //}
            Task<object> DatabaseFactory(BinaryReader r, FileMetadata f, EstatePakFile s)
            {
                var file = new DatabaseFile();
                file.Read(r);
                return Task.FromResult((object)file);
            }
            return Path.GetExtension(source.Path).ToLowerInvariant() switch
            {
                var x when x == ".cfg" || x == ".txt" || x == ".xml" => BinaryText.Factory,
                ".dds" => BinaryDds.Factory,
                //".dds" => DdsFactory,
                ".dcb" => DatabaseFactory,
                var x when x == ".soc" || x == ".cgf" || x == ".cga" || x == ".chr" || x == ".skin" || x == ".anim" => CryFile.Factory,
                _ => null,
            };
        }
    }
}