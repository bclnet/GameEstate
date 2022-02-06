using System.Runtime.InteropServices;

namespace System.Numerics
{
    // MARK Vector2

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Int2 { public int X; public int Y; public override string ToString() => $"{X},{Y}"; }

    // MARK Vector3

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Int3 { public int X; public int Y; public int Z; public Int3(int x, int y, int z) { X = x; Y = y; Z = z; } public override string ToString() => $"{X},{Y},{Z}"; }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Float3 { public float X; public float Y; public float Z; public override string ToString() => $"{X},{Y},{Z}"; public Vector3 ToVector3() => new Vector3(X, Y, Z); }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Byte3 { public byte X; public byte Y; public byte Z; public override string ToString() => $"{X},{Y},{Z}"; }

    public static class Polyfill
    {
        public static readonly float EPSILON = 0.00019999999f;

        public static bool IsZero(this Vector3 v) => v.X == 0 && v.Y == 0 && v.Z == 0;

        public static bool IsZeroEpsilon(this Vector3 v) => Math.Abs(v.X) <= EPSILON && Math.Abs(v.Y) <= EPSILON && Math.Abs(v.Z) <= EPSILON;

        public static bool NearZero(this Vector3 v) => Math.Abs(v.X) <= 1.0f && Math.Abs(v.Y) <= 1.0f && Math.Abs(v.Z) <= 1.0f;

        public static float Get(this Matrix4x4 source, int row, int column)
        {
            if (row == 0)
            {
                if (column == 0) return source.M11;
                else if (column == 1) return source.M12;
                else if (column == 2) return source.M13;
                else if (column == 3) return source.M14;
                else throw new ArgumentOutOfRangeException(nameof(row));
            }
            else if (row == 1)
            {
                if (column == 0) return source.M21;
                else if (column == 1) return source.M22;
                else if (column == 2) return source.M23;
                else if (column == 3) return source.M24;
                else throw new ArgumentOutOfRangeException(nameof(row));
            }
            else if (row == 2)
            {
                if (column == 0) return source.M31;
                else if (column == 1) return source.M32;
                else if (column == 2) return source.M33;
                else if (column == 3) return source.M34;
                else throw new ArgumentOutOfRangeException(nameof(row));
            }
            else if (row == 3)
            {
                if (column == 0) return source.M41;
                else if (column == 1) return source.M42;
                else if (column == 2) return source.M43;
                else if (column == 3) return source.M44;
                else throw new ArgumentOutOfRangeException(nameof(row));
            }
            else throw new ArgumentOutOfRangeException(nameof(column));
        }

        public static void Set(this Matrix4x4 source, int row, int column, float value)
        {
            if (row == 0)
            {
                if (column == 0) source.M11 = value;
                else if (column == 1) source.M12 = value;
                else if (column == 2) source.M13 = value;
                else if (column == 3) source.M14 = value;
                else throw new ArgumentOutOfRangeException(nameof(row));
            }
            else if (row == 1)
            {
                if (column == 0) source.M21 = value;
                else if (column == 1) source.M22 = value;
                else if (column == 2) source.M23 = value;
                else if (column == 3) source.M24 = value;
                else throw new ArgumentOutOfRangeException(nameof(row));
            }
            else if (row == 2)
            {
                if (column == 0) source.M31 = value;
                else if (column == 1) source.M32 = value;
                else if (column == 2) source.M33 = value;
                else if (column == 3) source.M34 = value;
                else throw new ArgumentOutOfRangeException(nameof(row));
            }
            else if (row == 3)
            {
                if (column == 0) source.M41 = value;
                else if (column == 1) source.M42 = value;
                else if (column == 2) source.M43 = value;
                else if (column == 3) source.M44 = value;
                else throw new ArgumentOutOfRangeException(nameof(row));
            }
            else throw new ArgumentOutOfRangeException(nameof(column));
        }
    }
}