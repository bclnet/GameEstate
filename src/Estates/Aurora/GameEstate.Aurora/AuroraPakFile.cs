using GameEstate.Aurora.Formats;
using GameEstate.Explorer;
using GameEstate.Formats;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameEstate.Aurora
{
    /// <summary>
    /// AuroraPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Formats.BinaryPakFile" />
    public class AuroraPakFile : BinaryPakManyFile
    {
        public class PakBinaryAuroraZip : AbstractPakBinaryZip2
        {
            public static readonly PakBinary Instance = new PakBinaryAuroraZip();
            protected override Func<BinaryReader, FileMetadata, Task<object>> GetObjectFactory(FileMetadata source) => null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public AuroraPakFile(Estate estate, string game, string filePath, object tag = null) : base(estate, game, filePath, filePath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) ? PakBinaryAuroraZip.Instance : PakBinaryAurora.Instance, tag)
        {
            ExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            Open();
        }
    }
}