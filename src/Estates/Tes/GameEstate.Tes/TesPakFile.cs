using GameEstate.Explorer;
using GameEstate.Formats;
using GameEstate.Formats.Unknown;
using GameEstate.Tes.Formats;
using GameEstate.Tes.Transforms;
using GameEstate.Transforms;
using OpenStack.Graphics;
using System.IO;
using System.Threading.Tasks;
using static GameEstate.EstateDebug;

namespace GameEstate.Tes
{
    /// <summary>
    /// TesPakFile
    /// </summary>
    /// <seealso cref="GameEstate.Formats.BinaryPakFile" />
    public class TesPakFile : BinaryPakManyFile, ITransformFileObject<IUnknownFileModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TesPakFile" /> class.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="game">The game.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="tag">The tag.</param>
        public TesPakFile(Estate estate, string game, string filePath, object tag = null)
            : base(estate, game, filePath, Path.GetExtension(filePath) != ".esm" ? PakBinaryTes.Instance : PakBinaryTesEsm.Instance, tag)
        {
            GetExplorerItems = StandardExplorerItem.GetPakFilesAsync;
            GetObjectFactoryFactory = FormatExtensions.GetObjectFactoryFactory;
            PathFinders.Add(typeof(ITextureInfo), FindTexture);
            Open();
        }

        #region PathFinders

        /// <summary>
        /// Finds the actual path of a texture.
        /// </summary>
        public string FindTexture(string path)
        {
            var textureName = Path.GetFileNameWithoutExtension(path);
            var textureNameInTexturesDir = $"textures/{textureName}";
            var filePath = $"{textureNameInTexturesDir}.dds";
            if (Contains(filePath)) return filePath;
            //filePath = $"{textureNameInTexturesDir}.tga";
            //if (Contains(filePath)) return filePath;
            var texturePathWithoutExtension = $"{Path.GetDirectoryName(path)}/{textureName}";
            filePath = $"{texturePathWithoutExtension}.dds";
            if (Contains(filePath)) return filePath;
            //filePath = $"{texturePathWithoutExtension}.tga";
            //if (Contains(filePath)) return filePath;
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