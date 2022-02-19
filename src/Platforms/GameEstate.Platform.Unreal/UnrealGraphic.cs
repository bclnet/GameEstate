using OpenStack.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameEstate
{
    public interface IUnrealGraphic : IOpenGraphic<object, object, int, object> { }

    public class UnrealGraphic : IUnrealGraphic
    {
        readonly EstatePakFile _source;

        public UnrealGraphic(EstatePakFile source)
        {
            _source = source;
        }

        public EstatePakFile Source => _source;
        public ITextureManager<int> TextureManager => throw new NotImplementedException();
        public IMaterialManager<object, int> MaterialManager => throw new NotImplementedException();
        public IShaderManager<object> ShaderManager => throw new NotImplementedException();
        public int LoadTexture(string path, out IDictionary<string, object> data) => throw new NotImplementedException();
        public void PreloadTexture(string path) => throw new NotImplementedException();
        public object CreateObject(string path, out IDictionary<string, object> data) => throw new NotImplementedException();
        public void PreloadObject(string path) => throw new NotImplementedException();
        public object LoadShader(string path, IDictionary<string, bool> args = null) => throw new NotImplementedException();

        public Task<T> LoadFileObjectAsync<T>(string path) => _source.LoadFileObjectAsync<T>(path);
    }
}