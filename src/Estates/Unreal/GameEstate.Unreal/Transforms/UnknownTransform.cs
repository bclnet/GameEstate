using System;
using GameEstate.Formats.Unknown;
using System.Threading.Tasks;

namespace GameEstate.Unreal.Transforms
{
    /// <summary>
    /// UnknownTransform
    /// </summary>
    public static class UnknownTransform
    {
        internal static bool CanTransformFileObject(EstatePakFile left, EstatePakFile right, object source) => throw new NotImplementedException();
        internal static Task<IUnknownFileModel> TransformFileObjectAsync(EstatePakFile left, EstatePakFile right, object source) => throw new NotImplementedException();
    }
}