using GameEstate.Graphics;
using GameEstate.Graphics.MaterialBuilders;
using GameEstate.Graphics.ObjectBuilders;
using GameEstate.Graphics.OpenGL;
using GameEstate.Graphics.ShaderBuilders;
using GameEstate.Graphics.TextureBuilders;
using System.Collections.Generic;

namespace GameEstate
{
    //public interface IOpenGLGraphic : IEstateGraphic<object, Material, int, Shader> { }

    public class OpenGLGraphic : IOpenGLGraphic
    {
        readonly EstatePakFile _source;
        readonly TextureManager<int> _textureManager;
        readonly MaterialManager<Material, int> _materialManager;
        readonly ObjectManager<object, Material, int> _objectManager;
        readonly ShaderManager<Shader> _shaderManager;

        public OpenGLGraphic(EstatePakFile source)
        {
            _source = source;
            _textureManager = new TextureManager<int>(source, new OpenGLTextureBuilder());
            _materialManager = new MaterialManager<Material, int>(source, _textureManager, new OpenGLMaterialBuilder(_textureManager));
            _objectManager = new ObjectManager<object, Material, int>(source, _materialManager, new OpenGLObjectBuilder());
            _shaderManager = new ShaderManager<Shader>(source, new OpenGLShaderBuilder());
            MeshBufferCache = new GpuMeshBufferCache();
        }

        public EstatePakFile Source => _source;
        public TextureManager<int> TextureManager => _textureManager;
        public MaterialManager<Material, int> MaterialManager => _materialManager;
        public ShaderManager<Shader> ShaderManager => _shaderManager;
        public int LoadTexture(string path, out IDictionary<string, object> data) => _textureManager.LoadTexture(path, out data);
        public void PreloadTexture(string path) => _textureManager.PreloadTexture(path);
        public object CreateObject(string path, out IDictionary<string, object> data) => _objectManager.CreateObject(path, out data);
        public void PreloadObject(string path) => _objectManager.PreloadObject(path);
        public Shader LoadShader(string path, IDictionary<string, bool> args = null) => _shaderManager.LoadShader(path, args);

        // cache
        QuadIndexBuffer _quadIndices;
        public QuadIndexBuffer QuadIndices => _quadIndices != null ? _quadIndices : _quadIndices = new QuadIndexBuffer(65532);
        public GpuMeshBufferCache MeshBufferCache { get; }
    }
}