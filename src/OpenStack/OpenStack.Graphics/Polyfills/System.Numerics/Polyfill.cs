using System.Globalization;
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
        public static Vector3 ParseVector(string input)
        {
            var split = input.Split(' ');
            return split.Length == 3
                ? new Vector3(float.Parse(split[0], CultureInfo.InvariantCulture), float.Parse(split[1], CultureInfo.InvariantCulture), float.Parse(split[2], CultureInfo.InvariantCulture))
                : default;
        }

        /// <summary>
        /// Flatten an array of matrices to an array of floats.
        /// </summary>
        public static float[] Flatten(this Matrix4x4[] matrices)
        {
            var r = new float[matrices.Length * 16];
            for (var i = 0; i < matrices.Length; i++)
            {
                var mat = matrices[i];
                r[i * 16] = mat.M11;
                r[(i * 16) + 1] = mat.M12;
                r[(i * 16) + 2] = mat.M13;
                r[(i * 16) + 3] = mat.M14;
                r[(i * 16) + 4] = mat.M21;
                r[(i * 16) + 5] = mat.M22;
                r[(i * 16) + 6] = mat.M23;
                r[(i * 16) + 7] = mat.M24;
                r[(i * 16) + 8] = mat.M31;
                r[(i * 16) + 9] = mat.M32;
                r[(i * 16) + 10] = mat.M33;
                r[(i * 16) + 11] = mat.M34;
                r[(i * 16) + 12] = mat.M41;
                r[(i * 16) + 13] = mat.M42;
                r[(i * 16) + 14] = mat.M43;
                r[(i * 16) + 15] = mat.M44;
            }
            return r;
        }

        public static Vector3 GetTranslation(this Matrix4x4 source) => new Vector3
        {
            X = source.M14,
            Y = source.M24,
            Z = source.M34
        };

        /// <summary>
        /// Gets the Rotation portion of a Transform Matrix44 (upper left).
        /// </summary>
        /// <returns>New Matrix33 with the rotation component.</returns>
        public static Matrix3x3 GetRotation(this Matrix4x4 source) => new Matrix3x3()
        {
            M11 = source.M11,
            M12 = source.M12,
            M13 = source.M13,
            M21 = source.M21,
            M22 = source.M22,
            M23 = source.M23,
            M31 = source.M31,
            M32 = source.M32,
            M33 = source.M33,
        };

        public static Vector3 GetScale(this Matrix4x4 source) => new Vector3
        {
            X = source.M41 / 100f,
            Y = source.M42 / 100f,
            Z = source.M43 / 100f
        };

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