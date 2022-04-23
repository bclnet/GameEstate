using GameEstate.Cry.Formats;
using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;
using static GameEstate.Estate;

namespace GameEstate.Rsi.Formats
{
    /// <summary>
    /// FormatExtensions
    /// </summary>
    public static class FormatExtensions
    {
        // object factory
        internal static (DataOption, Func<BinaryReader, FileMetadata, EstatePakFile, Task<object>>) GetObjectFactoryFactory(this FileMetadata source)
            => Path.GetExtension(source.Path).ToLowerInvariant() switch
            {
                var x when x == ".cfg" || x == ".txt" => (0, BinaryText.Factory),
                var x when x == ".xml" => (DataOption.Stream, CryXmlFile.Factory),
                ".dds" => (0, BinaryDds.Factory),
                ".dcb" => (0, ForgeFile.Factory),
                var x when x == ".soc" || x == ".cgf" || x == ".cga" || x == ".chr" || x == ".skin" || x == ".anim" => (DataOption.Model, CryFile.Factory),
                _ => (0, null),
            };
    }
}