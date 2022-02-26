using GameEstate.Formats.Unknown;
using System.Threading.Tasks;

namespace GameEstate.Rsi.Transforms
{
    /// <summary>
    /// UnknownTransform
    /// </summary>
    public static class UnknownTransform
    {
        internal static bool CanTransformFileObject(EstatePakFile left, EstatePakFile right, object source) => Cry.Transforms.UnknownTransform.CanTransformFileObject(left, right, source);
        internal static Task<IUnknownFileModel> TransformFileObjectAsync(EstatePakFile left, EstatePakFile right, object source) => Cry.Transforms.UnknownTransform.TransformFileObjectAsync(left, right, source);
    }
}