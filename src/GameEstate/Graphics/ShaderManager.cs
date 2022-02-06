using OpenStack.Graphics;
using System.Collections.Generic;

namespace GameEstate.Graphics
{
    public class ShaderManager<Shader> : IShaderManager<Shader>
    {
        static readonly Dictionary<string, bool> EmptyArgs = new Dictionary<string, bool>();
        readonly EstatePakFile _pakFile;
        readonly AbstractShaderBuilder<Shader> _builder;

        public ShaderManager(EstatePakFile pakFile, AbstractShaderBuilder<Shader> builder)
        {
            _pakFile = pakFile;
            _builder = builder;
        }

        public Shader LoadShader(string path, IDictionary<string, bool> args = null)
            => _builder.BuildShader(path, args ?? EmptyArgs);

        public Shader LoadPlaneShader(string path, IDictionary<string, bool> args = null)
            => _builder.BuildPlaneShader(path, args ?? EmptyArgs);
    }
}