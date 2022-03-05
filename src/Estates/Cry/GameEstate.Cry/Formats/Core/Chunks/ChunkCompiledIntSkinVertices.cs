using static GameEstate.EstateDebug;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public abstract class ChunkCompiledIntSkinVertices : Chunk
    {
        public int Reserved;
        public IntSkinVertex[] IntSkinVertices;
        public int NumIntVertices { get; set; } // Calculate by size of data div by size of IntSkinVertex structure.

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