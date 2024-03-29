﻿using System.Collections.Generic;

namespace GameEstate.Formats.Unknown
{
    public interface IUnknownFileObject
    {
        public struct Source
        {
            public string Author;
            public string SourceFile;
        }

        string Name { get; }
        string Path { get; }
        IEnumerable<Source> Sources { get; }
    }
}