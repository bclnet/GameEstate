﻿using static GameEstate.EstateDebug;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public abstract class ChunkCompiledMorphTargets : Chunk
    {
        public uint NumberOfMorphTargets;
        public MeshMorphTargetVertex[] MorphTargetVertices;

        #region Log
#if LOG
        public override void LogChunk()
        {
            Log($"*** START MorphTargets Chunk ***");
            Log($"    ChunkType:           {ChunkType}");
            Log($"    Node ID:             {ID:X}");
            Log($"    Number of Targets:   {NumberOfMorphTargets:X}");
        }
#endif
        #endregion
    }
}