using GameEstate.Graphics;
using GameEstate.Graphics.MaterialBuilders;
using GameEstate.Graphics.ObjectBuilders;
using GameEstate.Graphics.ShaderBuilders;
using GameEstate.Graphics.TextureBuilders;
using System.Collections.Generic;
using UnityEngine;

namespace GameEstate
{
    public interface IUnityGraphic : IEstateGraphic<GameObject, Material, Texture2D, Shader> { }

    public class UnityGraphic : IUnityGraphic
    {
        readonly EstatePakFile _source;
        readonly TextureManager<Texture2D> _textureManager;
        readonly MaterialManager<Material, Texture2D> _materialManager;
        readonly ObjectManager<GameObject, Material, Texture2D> _objectManager;
        readonly ShaderManager<Shader> _shaderManager;

        public UnityGraphic(EstatePakFile source)
        {
            _source = source;
            _textureManager = new TextureManager<Texture2D>(source, new UnityTextureBuilder());
            //switch (MaterialType.Default)
            //{
            //    case MaterialType.None: _material = null; break;
            //    case MaterialType.Default: _material = new DefaultMaterial(_textureManager); break;
            //    case MaterialType.Standard: _material = new StandardMaterial(_textureManager); break;
            //    case MaterialType.Unlit: _material = new UnliteMaterial(_textureManager); break;
            //    default: _material = new BumpedDiffuseMaterial(_textureManager); break;
            //}
            _materialManager = new MaterialManager<Material, Texture2D>(source, _textureManager, new BumpedDiffuseMaterialBuilder(_textureManager));
            _objectManager = new ObjectManager<GameObject, Material, Texture2D>(source, _materialManager, new UnityObjectBuilder(0));
            _shaderManager = new ShaderManager<Shader>(source, new UnityShaderBuilder());
        }

        public EstatePakFile Source => _source;
        public TextureManager<Texture2D> TextureManager => _textureManager;
        public MaterialManager<Material, Texture2D> MaterialManager => _materialManager;
        public ShaderManager<Shader> ShaderManager => _shaderManager;
        public Texture2D LoadTexture(string path, out IDictionary<string, object> data) => _textureManager.LoadTexture(path, out data);
        public void PreloadTexture(string path) => _textureManager.PreloadTexture(path);
        public GameObject CreateObject(string path, out IDictionary<string, object> data) => _objectManager.CreateObject(path, out data);
        public void PreloadObject(string path) => _objectManager.PreloadObject(path);
        public Shader LoadShader(string path, IDictionary<string, bool> args = null) => _shaderManager.LoadShader(path, args);
    }
}