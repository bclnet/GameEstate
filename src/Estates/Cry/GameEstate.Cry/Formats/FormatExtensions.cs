﻿using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;
using static GameEstate.Estate;

namespace GameEstate.Cry.Formats
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
                var x when x == ".xml" => (0, CryXmlFile.Factory),
                ".dds" => (0, BinaryDds.Factory),
                var x when x == ".cgf" || x == ".cga" || x == ".chr" || x == ".skin" || x == ".anim" => (0, CryFile.Factory),
                _ => (0, null),
            };
    }
}
