using GameEstate.Explorer;
using GameEstate.Formats;
using GameEstate.Formats.Unknown;
using GameEstate.Transforms;
using GameEstate.Valve.Formats;
using GameEstate.Valve.Transforms;
using System;
using System.Threading.Tasks;
using static GameEstate.EstateDebug;

namespace GameEstate.Valve
{
    /// <summary>
    /// ValvePakFile
    /// </summary>
    /// <seealso cref="GameEstate.Formats.BinaryPakFile" />
    public class ValvePakFile : BinaryPakManyFile, ITransformFileObject<IUnknownFileModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValvePakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public ValvePakFile(Estate estate, string game, string filePath, object tag = null)
            : base(estate, game, filePath, PakBinaryValve.Instance, tag)
        {
            GetExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            GetObjectFactoryFactory = FormatExtensions.GetObjectFactoryFactory;
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

        #region Transforms

        bool ITransformFileObject<IUnknownFileModel>.CanTransformFileObject(EstatePakFile transformTo, object source) => UnknownTransform.CanTransformFileObject(this, transformTo, source);
        Task<IUnknownFileModel> ITransformFileObject<IUnknownFileModel>.TransformFileObjectAsync(EstatePakFile transformTo, object source) => UnknownTransform.TransformFileObjectAsync(this, transformTo, source);

        #endregion
    }
}