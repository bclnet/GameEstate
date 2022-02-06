using OpenStack.Graphics;
using System;
using System.Threading.Tasks;

namespace GameEstate
{
    public interface ITestGraphic : IOpenGraphic { }

    public class TestGraphic : ITestGraphic
    {
        readonly EstatePakFile _source;

        public TestGraphic(EstatePakFile source) => _source = source;
        public object Source => _source;
        public Task<T> LoadFileObjectAsync<T>(string path) => throw new NotSupportedException();
        public void PreloadTexture(string texturePath) => throw new NotSupportedException();
        public void PreloadObject(string filePath) => throw new NotSupportedException();
    }
}