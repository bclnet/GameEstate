using GameEstate.AC.Formats;
using GameEstate.Explorer;
using GameEstate.Formats;

namespace GameEstate.AC
{
    /// <summary>
    /// ACPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Core.BinaryPakFile" />
    public class ACPakFile : BinaryPakManyFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ACPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public ACPakFile(Estate estate, string game, string filePath, object tag = null) : base(estate, game, filePath, PakBinaryAC.Instance, tag)
        {
            ExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            Open();
        }
    }
}