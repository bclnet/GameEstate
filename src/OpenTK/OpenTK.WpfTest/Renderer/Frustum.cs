using System;
using System.Numerics;
using NVector4 = System.Numerics.Vector4;

namespace OpenTK.WpfTest.Renderer
{
    public class Frustum
    {
        NVector4[] Planes = new NVector4[6];

        public static Frustum CreateEmpty() => new() { Planes = Array.Empty<NVector4>() };

        public void Update(Matrix4x4 viewProjectionMatrix)
        {
            Planes[0] = NVector4.Normalize(new NVector4(
                viewProjectionMatrix.M14 + viewProjectionMatrix.M11,
                viewProjectionMatrix.M24 + viewProjectionMatrix.M21,
                viewProjectionMatrix.M34 + viewProjectionMatrix.M31,
                viewProjectionMatrix.M44 + viewProjectionMatrix.M41));
            Planes[1] = NVector4.Normalize(new NVector4(
                viewProjectionMatrix.M14 - viewProjectionMatrix.M11,
                viewProjectionMatrix.M24 - viewProjectionMatrix.M21,
                viewProjectionMatrix.M34 - viewProjectionMatrix.M31,
                viewProjectionMatrix.M44 - viewProjectionMatrix.M41));
            Planes[2] = NVector4.Normalize(new NVector4(
                viewProjectionMatrix.M14 - viewProjectionMatrix.M12,
                viewProjectionMatrix.M24 - viewProjectionMatrix.M22,
                viewProjectionMatrix.M34 - viewProjectionMatrix.M32,
                viewProjectionMatrix.M44 - viewProjectionMatrix.M42));
            Planes[3] = NVector4.Normalize(new NVector4(
                viewProjectionMatrix.M14 + viewProjectionMatrix.M12,
                viewProjectionMatrix.M24 + viewProjectionMatrix.M22,
                viewProjectionMatrix.M34 + viewProjectionMatrix.M32,
                viewProjectionMatrix.M44 + viewProjectionMatrix.M42));
            Planes[4] = NVector4.Normalize(new NVector4(
                viewProjectionMatrix.M13,
                viewProjectionMatrix.M23,
                viewProjectionMatrix.M33,
                viewProjectionMatrix.M43));
            Planes[5] = NVector4.Normalize(new NVector4(
                viewProjectionMatrix.M14 - viewProjectionMatrix.M13,
                viewProjectionMatrix.M24 - viewProjectionMatrix.M23,
                viewProjectionMatrix.M34 - viewProjectionMatrix.M33,
                viewProjectionMatrix.M44 - viewProjectionMatrix.M43));
        }

        public Frustum Clone()
        {
            var rv = new Frustum();
            Planes.CopyTo(rv.Planes, 0);
            return rv;
        }

        //public bool Intersects(AABB box)
        //{
        //    for (var i = 0; i < Planes.Length; ++i)
        //    {
        //        var closest = new Vector3(
        //            Planes[i].X < 0 ? box.Min.X : box.Max.X,
        //            Planes[i].Y < 0 ? box.Min.Y : box.Max.Y,
        //            Planes[i].Z < 0 ? box.Min.Z : box.Max.Z);
        //        if (Vector3.Dot(new Vector3(Planes[i].X, Planes[i].Y, Planes[i].Z), closest) + Planes[i].W < 0) return false;
        //    }
        //    return true;
        //}
    }
}
