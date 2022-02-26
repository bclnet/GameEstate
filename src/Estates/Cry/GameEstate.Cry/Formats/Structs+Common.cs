using MathNet.Numerics.LinearAlgebra;
using System;
using static GameEstate.EstateDebug;

namespace GameEstate.Cry.Formats
{
    /// <summary>
    /// Vector in 3D space {x,y,z}
    /// </summary>
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public float w; // Currently Unused

        public Vector3(float x, float y, float z) : this() { this.x = x; this.y = y; this.z = z; }
        public Vector3 Add(Vector3 vector) => new Vector3 { x = vector.x + x, y = vector.y + y, z = vector.z + z };
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs) => new Vector3 { x = lhs.x + rhs.x, y = lhs.y + rhs.y, z = lhs.z + rhs.z };
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs) => new Vector3 { x = lhs.x - rhs.x, y = lhs.y - rhs.y, z = lhs.z - rhs.z };
        public Vector4 ToVector4() => new Vector4 { x = x, y = y, z = z, w = 1 };
    }

    public struct Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector4(float x, float y, float z, float w) { this.x = x; this.y = y; this.z = z; this.w = w; }
        public Vector3 ToVector3() { var r = new Vector3(); if (w == 0) { r.x = x; r.y = y; r.z = z; } else { r.x = x / w; r.y = y / w; r.z = z / w; } return r; }
    }

    public struct Matrix3x3    // a 3x3 transformation matrix
    {
        public float m00;
        public float m01;
        public float m02;
        public float m10;
        public float m11;
        public float m12;
        public float m20;
        public float m21;
        public float m22;

        /// <summary>
        /// Determines whether this instance is identity.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is identity; otherwise, <c>false</c>.
        /// </returns>
        public bool IsIdentity() =>
            Math.Abs(m00 - 1.0) > 0.00001 ||
            Math.Abs(m01) > 0.00001 ||
            Math.Abs(m02) > 0.00001 ||
            Math.Abs(m10) > 0.00001 ||
            Math.Abs(m11 - 1.0) > 0.00001 ||
            Math.Abs(m12) > 0.00001 ||
            Math.Abs(m20) > 0.00001 ||
            Math.Abs(m21) > 0.00001 ||
            Math.Abs(m22 - 1.0) > 0.00001
                ? false
                : true;

        /// <summary>
        /// Gets the copy.
        /// </summary>
        /// <returns>copy of the matrix33</returns>
        public Matrix3x3 GetCopy() => new Matrix3x3
        {
            m00 = m00,
            m01 = m01,
            m02 = m02,
            m10 = m10,
            m11 = m11,
            m12 = m12,
            m20 = m20,
            m21 = m21,
            m22 = m22
        };

        public float GetDeterminant() =>
            m00 * m11 * m22
            + m01 * m12 * m20
            + m02 * m10 * m21
            - m20 * m11 * m02
            - m10 * m01 * m22
            - m00 * m21 * m12;

        /// <summary>
        /// Gets the transpose.
        /// </summary>
        /// <returns>copy of the matrix33</returns>
        public Matrix3x3 GetTranspose() => new Matrix3x3
        {
            m00 = m00,
            m01 = m10,
            m02 = m20,
            m10 = m01,
            m11 = m11,
            m12 = m21,
            m20 = m02,
            m21 = m12,
            m22 = m22
        };

        public Matrix3x3 Mult(Matrix3x3 mat) => new Matrix3x3
        {
            m00 = (m00 * mat.m00) + (m01 * mat.m10) + (m02 * mat.m20),
            m01 = (m00 * mat.m01) + (m01 * mat.m11) + (m02 * mat.m21),
            m02 = (m00 * mat.m02) + (m01 * mat.m12) + (m02 * mat.m22),
            m10 = (m10 * mat.m00) + (m11 * mat.m10) + (m12 * mat.m20),
            m11 = (m10 * mat.m01) + (m11 * mat.m11) + (m12 * mat.m21),
            m12 = (m10 * mat.m02) + (m11 * mat.m12) + (m12 * mat.m22),
            m20 = (m20 * mat.m00) + (m21 * mat.m10) + (m22 * mat.m20),
            m21 = (m20 * mat.m01) + (m21 * mat.m11) + (m22 * mat.m21),
            m22 = (m20 * mat.m02) + (m21 * mat.m12) + (m22 * mat.m22)
        };

        public static Matrix3x3 operator *(Matrix3x3 lhs, Matrix3x3 rhs) => lhs.Mult(rhs);

        /// <summary>
        /// Multiply the 3x3 matrix by a Vector 3 to get the rotation
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns></returns>
        public Vector3 Mult3x1(Vector3 vector) => new Vector3
        {
            x = (vector.x * m00) + (vector.y * m10) + (vector.z * m20),
            y = (vector.x * m01) + (vector.y * m11) + (vector.z * m21),
            z = (vector.x * m02) + (vector.y * m12) + (vector.z * m22)
        };

        public static Vector3 operator *(Matrix3x3 rhs, Vector3 lhs) => rhs.Mult3x1(lhs);

        /// <summary>
        /// Determines whether the matrix decomposes nicely into scale * rotation.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is scale rotation]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsScaleRotation()
        {
            var transpose = GetTranspose();
            var mat = Mult(transpose);
            if (Math.Abs(mat.m01) + Math.Abs(mat.m02)
                + Math.Abs(mat.m10) + Math.Abs(mat.m12)
                + Math.Abs(mat.m20) + Math.Abs(mat.m21) > 0.01) { Log(" is a Scale_Rot matrix"); return false; }
            Log(" is not a Scale_Rot matrix");
            return true;
        }

        /// <summary>
        /// Get the scale, assuming IsScaleRotation is true
        /// </summary>
        /// <returns></returns>
        public Vector3 GetScale()
        {
            var mat = Mult(GetTranspose());
            var scale = new Vector3
            {
                x = (float)Math.Pow(mat.m00, 0.5f),
                y = (float)Math.Pow(mat.m11, 0.5f),
                z = (float)Math.Pow(mat.m22, 0.5f)
            };
            if (GetDeterminant() < 0)
            {
                scale.x = 0 - scale.x;
                scale.y = 0 - scale.y;
                scale.z = 0 - scale.z;
                return scale;
            }
            return scale;
        }

        /// <summary>
        /// Gets the scale, should also return the rotation matrix, but..eh...
        /// </summary>
        /// <returns></returns>
        public Vector3 GetScaleRotation() => GetScale();

        public bool IsRotation()
        {
            // NOTE: 0.01 instead of CgfFormat.EPSILON to work around bad files
            if (!IsScaleRotation()) return false;
            var scale = GetScale();
            return Math.Abs(scale.x - 1.0f) > 0.01f || Math.Abs(scale.y - 1.0f) > 0.01f || Math.Abs(scale.z - 1.0f) > 0.1f ? false : true;
        }

        public float Determinant() => this.ToMathMatrix().Determinant();
        public Matrix3x3 Inverse() => this.ToMathMatrix().Inverse().ToMatrix3x3();
        public Matrix3x3 Conjugate() => this.ToMathMatrix().Conjugate().ToMatrix3x3();
        public Matrix3x3 ConjugateTranspose() => this.ToMathMatrix().ConjugateTranspose().ToMatrix3x3();
        public Matrix3x3 ConjugateTransposeThisAndMultiply(Matrix3x3 inputMatrix) => this.ToMathMatrix().ConjugateTransposeThisAndMultiply(inputMatrix.ToMathMatrix()).ToMatrix3x3();
        public Vector3 Diagonal() => new Vector3().ToVector3(this.ToMathMatrix().Diagonal());
    }

    /// <summary>
    /// A 4x4 Transformation matrix.  These are row major matrices (m13 is first row, 3rd column). [first value is row, second is column.]
    /// </summary>
    public struct Matrix4x4
    {
        public float m00;
        public float m01;
        public float m02;
        public float m03;
        public float m10;
        public float m11;
        public float m12;
        public float m13;
        public float m20;
        public float m21;
        public float m22;
        public float m23;
        public float m30;
        public float m31;
        public float m32;
        public float m33;

        /// <summary>
        /// Pass the matrix a Vector4 (4x1) vector to get the transform of the vector
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns></returns>
        public Vector4 Mult4x1(Vector4 vector) => new Vector4
        {
            x = (m00 * vector.x) + (m10 * vector.y) + (m20 * vector.z) + m30 / 100f,
            y = (m01 * vector.x) + (m11 * vector.y) + (m21 * vector.z) + m31 / 100f,
            z = (m02 * vector.x) + (m12 * vector.y) + (m22 * vector.z) + m32 / 100f,
            w = (m03 * vector.x) + (m13 * vector.y) + (m23 * vector.z) + m33 / 100f
        };

        public static Vector4 operator *(Matrix4x4 lhs, Vector4 vector) => new Vector4
        {
            x = (lhs.m00 * vector.x) + (lhs.m10 * vector.y) + (lhs.m20 * vector.z) + lhs.m30 / 100f,
            y = (lhs.m01 * vector.x) + (lhs.m11 * vector.y) + (lhs.m21 * vector.z) + lhs.m31 / 100f,
            z = (lhs.m02 * vector.x) + (lhs.m12 * vector.y) + (lhs.m22 * vector.z) + lhs.m32 / 100f,
            w = (lhs.m03 * vector.x) + (lhs.m13 * vector.y) + (lhs.m23 * vector.z) + lhs.m33 / 100f
        };

        public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs) => new Matrix4x4
        {
            // First row
            m00 = (lhs.m00 * rhs.m00) + (lhs.m01 * rhs.m10) + (lhs.m02 * rhs.m20) + (lhs.m03 * rhs.m30),
            m01 = (lhs.m00 * rhs.m01) + (lhs.m01 * rhs.m11) + (lhs.m02 * rhs.m21) + (lhs.m03 * rhs.m31),
            m02 = (lhs.m00 * rhs.m02) + (lhs.m01 * rhs.m12) + (lhs.m02 * rhs.m22) + (lhs.m03 * rhs.m32),
            m03 = (lhs.m00 * rhs.m03) + (lhs.m01 * rhs.m13) + (lhs.m02 * rhs.m23) + (lhs.m03 * rhs.m33),
            // second row
            m10 = (lhs.m10 * rhs.m00) + (lhs.m11 * rhs.m10) + (lhs.m12 * rhs.m20) + (lhs.m13 * rhs.m30),
            m11 = (lhs.m10 * rhs.m01) + (lhs.m11 * rhs.m11) + (lhs.m12 * rhs.m21) + (lhs.m13 * rhs.m31),
            m12 = (lhs.m10 * rhs.m02) + (lhs.m11 * rhs.m12) + (lhs.m12 * rhs.m22) + (lhs.m13 * rhs.m32),
            m13 = (lhs.m10 * rhs.m03) + (lhs.m11 * rhs.m13) + (lhs.m12 * rhs.m23) + (lhs.m13 * rhs.m33),
            // third row
            m20 = (lhs.m20 * rhs.m00) + (lhs.m21 * rhs.m10) + (lhs.m22 * rhs.m20) + (lhs.m23 * rhs.m30),
            m21 = (lhs.m20 * rhs.m01) + (lhs.m21 * rhs.m11) + (lhs.m22 * rhs.m21) + (lhs.m23 * rhs.m31),
            m22 = (lhs.m20 * rhs.m02) + (lhs.m21 * rhs.m12) + (lhs.m22 * rhs.m22) + (lhs.m23 * rhs.m32),
            m23 = (lhs.m20 * rhs.m03) + (lhs.m21 * rhs.m13) + (lhs.m22 * rhs.m23) + (lhs.m23 * rhs.m33),
            // fourth row
            m30 = (lhs.m30 * rhs.m00) + (lhs.m31 * rhs.m10) + (lhs.m32 * rhs.m20) + (lhs.m33 * rhs.m30),
            m31 = (lhs.m30 * rhs.m01) + (lhs.m31 * rhs.m11) + (lhs.m32 * rhs.m21) + (lhs.m33 * rhs.m31),
            m32 = (lhs.m30 * rhs.m02) + (lhs.m31 * rhs.m12) + (lhs.m32 * rhs.m22) + (lhs.m33 * rhs.m32),
            m33 = (lhs.m30 * rhs.m03) + (lhs.m31 * rhs.m13) + (lhs.m32 * rhs.m23) + (lhs.m33 * rhs.m33)
        };

        public Vector3 GetTranslation() => new Vector3
        {
            x = m03,
            y = m13,
            z = m23
        };

        /// <summary>
        /// Gets the Rotation portion of a Transform Matrix44 (upper left).
        /// </summary>
        /// <returns>New Matrix33 with the rotation component.</returns>
        public Matrix3x3 GetRotation() => new Matrix3x3()
        {
            m00 = m00,
            m01 = m01,
            m02 = m02,
            m10 = m10,
            m11 = m11,
            m12 = m12,
            m20 = m20,
            m21 = m21,
            m22 = m22,
        };

        public Vector3 GetScale() => new Vector3
        {
            x = m30 / 100f,
            y = m31 / 100f,
            z = m32 / 100f
        };

        public Vector3 GetBoneTranslation() => new Vector3
        {
            x = m03,
            y = m13,
            z = m23
        };

        public float[,] ConvertTo4x4Array()
        {
            var r = new float[4, 4];
            r[0, 0] = m00;
            r[0, 1] = m01;
            r[0, 2] = m02;
            r[0, 3] = m03;
            r[1, 0] = m10;
            r[1, 1] = m11;
            r[1, 2] = m12;
            r[1, 3] = m13;
            r[2, 0] = m20;
            r[2, 1] = m21;
            r[2, 2] = m22;
            r[2, 3] = m23;
            r[3, 0] = m30;
            r[3, 1] = m31;
            r[3, 2] = m32;
            r[3, 3] = m33;
            return r;
        }

        public Matrix4x4 Inverse() => this.ToMathMatrix().Inverse().ToMatrix4x4();

        public Matrix4x4 GetTransformFromParts(Vector3 localTranslation, Matrix3x3 localRotation, Vector3 localScale) => new Matrix4x4
        {
            // For Node Chunks, the translation appears to be along the bottom of the matrix, and scale on right side.
            // Translation part
            m30 = localTranslation.x,
            m31 = localTranslation.y,
            m32 = localTranslation.z,
            // Rotation part
            m00 = localRotation.m00,
            m01 = localRotation.m01,
            m02 = localRotation.m02,
            m10 = localRotation.m10,
            m11 = localRotation.m11,
            m12 = localRotation.m12,
            m20 = localRotation.m20,
            m21 = localRotation.m21,
            m22 = localRotation.m22,
            // Scale part
            m03 = localScale.x,
            m13 = localScale.y,
            m23 = localScale.z,
            // Set final row
            m33 = 1
        };

        public static Matrix4x4 Identity() => new Matrix4x4()
        {
            m00 = 1,
            m01 = 0,
            m02 = 0,
            m03 = 0,
            m10 = 0,
            m11 = 1,
            m12 = 0,
            m13 = 0,
            m20 = 0,
            m21 = 0,
            m22 = 1,
            m23 = 0,
            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1
        };
    }

    /// <summary>
    /// A quaternion (x,y,z,w)
    /// </summary>
    public struct Quat
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }
}