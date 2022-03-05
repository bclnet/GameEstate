using static GameEstate.EstateDebug;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    /// <summary>
    /// Legacy class.  No longer used.
    /// </summary>
    public abstract class ChunkMeshMorphTargets : Chunk
    {
        public uint ChunkIDMesh;
        public uint NumMorphVertices;

        #region Log
#if LOG
        public override void LogChunk()
        {
            Log($"*** START MorphTargets Chunk ***");
            Log($"    ChunkType:           {ChunkType}");
            Log($"    Node ID:             {ID:X}");
            Log($"    Chunk ID Mesh:       {ChunkIDMesh:X}");
        }
#endif
        #endregion
    }
}