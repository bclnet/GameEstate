using GameEstate.Arkane.Formats;
using GameEstate.Explorer;
using GameEstate.Formats;

namespace GameEstate.Arkane
{
    /// <summary>
    /// ArkanePakFile
    /// </summary>
    /// <seealso cref="GameEstate.Core.BinaryPakFile" />
    public class ArkanePakFile : BinaryPakManyFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArkanePakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public ArkanePakFile(Estate estate, string game, string filePath, object tag = null) : base(estate, game, filePath, PakBinaryArkane.Instance, tag)
        {
            ExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            Open();
        }
    }
}