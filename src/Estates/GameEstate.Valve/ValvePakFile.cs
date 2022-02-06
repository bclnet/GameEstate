using GameEstate.Explorer;
using GameEstate.Formats;
using GameEstate.Valve.Formats;
using System;
using static GameEstate.EstateDebug;

namespace GameEstate.Valve
{
    /// <summary>
    /// ValvePakFile
    /// </summary>
    /// <seealso cref="GameEstate.Core.BinaryPakFile" />
    public class ValvePakFile : BinaryPakManyFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValvePakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public ValvePakFile(Estate estate, string game, string filePath, object tag = null) : base(estate, game, filePath, PakBinaryValve.Instance, tag)
        {
            ExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            PathFinders.Add(typeof(object), FindBinary);
            Open();
        }

        #region PathFinders

        /// <summary>
        /// Finds the actual path of a texture.
        /// </summary>
        public string FindBinary(string path)
        {
            if (Contains(path)) return path;
            if (!path.EndsWith("_c", StringComparison.Ordinal)) path = $"{path}_c";
            if (Contains(path)) return path;
            Log($"Could not find file '{path}' in a PAK file.");
            return null;
        }

        #endregion
    }
}