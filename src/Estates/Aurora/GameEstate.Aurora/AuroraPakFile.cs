using GameEstate.Aurora.Formats;
using GameEstate.Aurora.Transforms;
using GameEstate.Explorer;
using GameEstate.Formats;
using GameEstate.Formats.Unknown;
using GameEstate.Transforms;
using System;
using System.Threading.Tasks;

namespace GameEstate.Aurora
{
    /// <summary>
    /// AuroraPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Formats.BinaryPakFile" />
    public class AuroraPakFile : BinaryPakManyFile, ITransformFileObject<IUnknownFileModel>
    {
        public static readonly PakBinary ZipInstance = new PakBinarySystemZip();

        /// <summary>
        /// Initializes a new instance of the <see cref="AuroraPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public AuroraPakFile(Estate estate, string game, string filePath, object tag = null)
            : base(estate, game, filePath, filePath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) ? ZipInstance : PakBinaryAurora.Instance, tag)
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