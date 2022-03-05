﻿using static GameEstate.EstateDebug;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public abstract class ChunkCompiledExtToIntMap : Chunk
    {
        public int Reserved;
        public uint NumExtVertices;
        public ushort[] Source;

        public override string ToString()
            => $@"Chunk Type: {ChunkType}, ID: {ID:X}";

        #region Log
#if LOG
        public override void LogChunk()
        {
            Log($"*** START MorphTargets Chunk ***");
            Log($"    ChunkType:           {ChunkType}");
            Log($"    Node ID:             {ID:X}");
        }
#endif
        #endregion
    }
}