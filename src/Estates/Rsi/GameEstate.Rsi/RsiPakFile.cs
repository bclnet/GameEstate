using GameEstate.Explorer;
using GameEstate.Formats;
using GameEstate.Formats.Unknown;
using GameEstate.Rsi.Formats;
using GameEstate.Rsi.Transforms;
using GameEstate.Transforms;
using System.Threading.Tasks;

namespace GameEstate.Rsi
{
    /// <summary>
    /// RsiPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Formats.BinaryPakFile" />
    public class RsiPakFile : BinaryPakManyFile, ITransformFileObject<IUnknownFileModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RsiPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public RsiPakFile(Estate estate, string game, string filePath, object tag = null)
            : base(estate, game, filePath, PakBinaryRsi.Instance, tag)
        {
            GetExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            GetObjectFactoryFactory = FormatExtensions.GetObjectFactoryFactory;
            Open();
        }

        #region Transforms

        bool ITransformFileObject<IUnknownFileModel>.CanTransformFileObject(EstatePakFile transformTo, object source) => UnknownTransform.CanTransformFileObject(this, transformTo, source);
        Task<IUnknownFileModel> ITransformFileObject<IUnknownFileModel>.TransformFileObjectAsync(EstatePakFile transformTo, object source) => UnknownTransform.TransformFileObjectAsync(this, transformTo, source);

        #endregion
    }
}