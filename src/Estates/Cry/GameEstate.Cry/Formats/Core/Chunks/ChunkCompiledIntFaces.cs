﻿using static OpenStack.Debug;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public abstract class ChunkCompiledIntFaces : Chunk
    {
        public int Reserved;
        public int NumIntFaces;
        public TFace[] Faces;

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