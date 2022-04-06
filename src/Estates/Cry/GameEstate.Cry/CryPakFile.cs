using GameEstate.Cry.Formats;
using GameEstate.Cry.Transforms;
using GameEstate.Explorer;
using GameEstate.Formats;
using GameEstate.Formats.Unknown;
using GameEstate.Transforms;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("GameEstate.Rsi")]

namespace GameEstate.Cry
{
    /// <summary>
    /// CryPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Formats.BinaryPakFile" />
    public class CryPakFile : BinaryPakManyFile, ITransformFileObject<IUnknownFileModel>
    {
        static ConcurrentDictionary<string, PakBinaryCry> PakBinarys = new ConcurrentDictionary<string, PakBinaryCry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CryPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public CryPakFile(Estate estate, string game, string filePath, object tag = null)
            : base(estate, game, filePath, GetPackBinary(estate, game), tag)
        {
            GetExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            GetObjectFactoryFactory = FormatExtensions.GetObjectFactoryFactory;
            Open();
        }

        static PakBinaryCry PackBinaryFactory(Estate.EstateGame game)
        {
            if (game.Key is Estate.AesKey aes)
                return new PakBinaryCry(aes.Key);
            return new PakBinaryCry(null);
        }

        static PakBinary GetPackBinary(Estate estate, string game)
           => PakBinarys.GetOrAdd(game, _ => PackBinaryFactory(estate.GetGame(game).game));

        #region Transforms

        bool ITransformFileObject<IUnknownFileModel>.CanTransformFileObject(EstatePakFile transformTo, object source) => UnknownTransform.CanTransformFileObject(this, transformTo, source);
        Task<IUnknownFileModel> ITransformFileObject<IUnknownFileModel>.TransformFileObjectAsync(EstatePakFile transformTo, object source) => UnknownTransform.TransformFileObjectAsync(this, transformTo, source);

        #endregion
    }
}