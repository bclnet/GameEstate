using static GameEstate.EstateDebug;

namespace GameEstate.Cry.Formats.Core.Chunks
{
    public abstract class ChunkSourceInfo : Chunk  // cccc0013:  Source Info chunk.  Pretty useless overall
    {
        public string SourceFile;
        public string Date;
        public string Author;

        #region Log
#if LOG
        public override void LogChunk()
        {
            Log($"*** SOURCE INFO CHUNK ***");
            Log($"    ID: {ID:X}");
            Log($"    Sourcefile: {SourceFile}.");
            Log($"    Date:       {Date}.");
            Log($"    Author:     {Author}.");
            Log($"*** END SOURCE INFO CHUNK ***");
        }
#endif
        #endregion
    }
}