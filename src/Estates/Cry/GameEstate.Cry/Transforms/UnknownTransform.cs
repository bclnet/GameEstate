using GameEstate.Cry.Formats;
using GameEstate.Formats.Unknown;
using System.Threading.Tasks;

namespace GameEstate.Cry.Transforms
{
    /// <summary>
    /// UnknownTransform
    /// </summary>
    public static class UnknownTransform
    {
        internal static bool CanTransformFileObject(EstatePakFile left, EstatePakFile right, object source) => source is CryFile;
        internal static Task<IUnknownFileModel> TransformFileObjectAsync(EstatePakFile left, EstatePakFile right, object source)
            => Task.FromResult((IUnknownFileModel)source);
    }
}