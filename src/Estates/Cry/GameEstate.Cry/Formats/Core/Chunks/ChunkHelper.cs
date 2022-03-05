using System.Numerics;
using static GameEstate.EstateDebug;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    /// <summary>
    /// Helper chunk.  This is the top level, then nodes, then mesh, then mesh subsets
    /// CCCC0001  
    /// </summary>
    public abstract class ChunkHelper : Chunk
    {
        public string Name;
        public HelperType HelperType;
        public Vector3 Pos;
        public Matrix4x4 Transform;

        #region Log
#if LOG
        public override void LogChunk()
        {
            Log($"*** START Helper Chunk ***");
            Log($"    ChunkType:   {ChunkType}");
            Log($"    Version:     {Version:X}");
            Log($"    ID:          {ID:X}");
            Log($"    HelperType:  {HelperType}");
            Log($"    Position:    {Pos.X}, {Pos.Y}, {Pos.Z}");
            Log($"*** END Helper Chunk ***");
        }
#endif
        #endregion
    }
}