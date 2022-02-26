using GameEstate.AC.Formats;
using GameEstate.AC.Transforms;
using GameEstate.Explorer;
using GameEstate.Formats;
using GameEstate.Formats.Unknown;
using GameEstate.Transforms;
using System.Text;
using System.Threading.Tasks;

namespace GameEstate.AC
{
    /// <summary>
    /// ACPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Formats.BinaryPakFile" />
    public class ACPakFile : BinaryPakManyFile, ITransformFileObject<IUnknownFileModel>
    {
        static ACPakFile() => Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        /// <summary>
        /// Initializes a new instance of the <see cref="ACPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public ACPakFile(Estate estate, string game, string filePath, object tag = null)
            : base(estate, game, filePath, PakBinaryAC.Instance, tag)
        {
            Options = PakManyOptions.FilesById;
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