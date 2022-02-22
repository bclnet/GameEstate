using GameEstate.Aurora.Formats;
using GameEstate.Explorer;
using GameEstate.Formats;
using System;

namespace GameEstate.Aurora
{
    /// <summary>
    /// AuroraPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Formats.BinaryPakFile" />
    public class AuroraPakFile : BinaryPakManyFile
    {
        public static readonly PakBinary ZipInstance = new PakBinaryZip2();

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public AuroraPakFile(Estate estate, string game, string filePath, object tag = null) : base(estate, game, filePath, filePath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) ? ZipInstance : PakBinaryAurora.Instance, tag)
        {
            ExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            Open();
        }
    }
}