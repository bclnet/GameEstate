using GameEstate.Explorer;
using GameEstate.Formats;

namespace GameEstate.Ukn
{
    /// <summary>
    /// UknPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Core.BinaryPakFile" />
    public class UknPakFile : BinaryPakManyFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UknPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public UknPakFile(Estate estate, string game, string filePath, object tag = null) : base(estate, game, filePath, null, tag)
        {
            ExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            Open();
        }
    }
}