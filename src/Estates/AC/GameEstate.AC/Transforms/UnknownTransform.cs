﻿using GameEstate.Formats.Unknown;
using System;
using System.Threading.Tasks;

namespace GameEstate.AC.Transforms
{
    /// <summary>
    /// UnknownTransform
    /// </summary>
    public static class UnknownTransform
    {
        internal static bool CanTransformFileObject(EstatePakFile left, EstatePakFile right, object source) => false;
        internal static Task<IUnknownFileModel> TransformFileObjectAsync(EstatePakFile left, EstatePakFile right, object source) => throw new NotImplementedException();
    }
}