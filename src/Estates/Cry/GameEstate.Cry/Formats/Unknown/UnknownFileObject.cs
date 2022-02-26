﻿using GameEstate.Cry.Formats.Core.Chunks;
using GameEstate.Formats.Unknown;
using System.Collections.Generic;
using System.Linq;

namespace GameEstate.Cry.Formats.Unknown
{
    public class UnknownFileObject : IUnknownFileObject
    {
        protected CryFile File { get; }

        public string Name => File.Name;
        public string Path => File.InputFile;
        public IEnumerable<IUnknownFileObject.Source> Sources
            => File.Chunks.Where(a => a.ChunkType == ChunkTypeEnum.SourceInfo).Select(x =>
            {
                var s = (ChunkSourceInfo)x;
                return new IUnknownFileObject.Source { Author = s.Author, SourceFile = s.SourceFile };
            });
    }
}
