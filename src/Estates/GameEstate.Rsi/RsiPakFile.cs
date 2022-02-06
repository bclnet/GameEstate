﻿using GameEstate.Explorer;
using GameEstate.Formats;
using GameEstate.Rsi.Formats;

namespace GameEstate.Rsi
{
    /// <summary>
    /// RsiPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Core.BinaryPakFile" />
    public class RsiPakFile : BinaryPakManyFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RsiPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public RsiPakFile(Estate estate, string game, string filePath, object tag = null) : base(estate, game, filePath, PakBinaryRsi.Instance, tag)
        {
            ExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            Open();
        }
    }
}