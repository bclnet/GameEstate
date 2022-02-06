using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenStack.Graphics
{
    /// <summary>
    /// TextureExtensions
    /// </summary>
    public static partial class TextureExtensions
    {
        static partial class Literal
        {
            public static readonly byte[] IMG_ = Encoding.ASCII.GetBytes("IMG ");
        }

        #region Size

        public static int GetMipMapTrueDataSize(this ITextureInfo source, int index)
        {
            var format = source.GLFormat;
            var bytesPerPixel = format.GetBlockSize();
            var currentWidth = source.Width >> index;
            var currentHeight = source.Height >> index;
            var currentDepth = source.Depth >> index;
            if (currentDepth < 1) currentDepth = 1;
            if (format == TextureGLFormat.DXT1 || format == TextureGLFormat.DXT5 || format == TextureGLFormat.BC6H || format == TextureGLFormat.BC7 ||
                format == TextureGLFormat.ETC2 || format == TextureGLFormat.ETC2_EAC || format == TextureGLFormat.ATI1N)
            {
                var misalign = currentWidth % 4;
                if (misalign > 0) currentWidth += 4 - misalign;
                misalign = currentHeight % 4;
                if (misalign > 0) currentHeight += 4 - misalign;
                if (currentWidth < 4 && currentWidth > 0) currentWidth = 4;
                if (currentHeight < 4 && currentHeight > 0) currentHeight = 4;
                if (currentDepth < 4 && currentDepth > 1) currentDepth = 4;
                var numBlocks = (currentWidth * currentHeight) >> 4;
                numBlocks *= currentDepth;
                return numBlocks * bytesPerPixel;
            }
            return currentWidth * currentHeight * currentDepth * bytesPerPixel;
        }

        public static int GetBlockSize(this TextureGLFormat source)
        {
            switch (source)
            {
                case TextureGLFormat.DXT1: return 8;
                case TextureGLFormat.DXT5: return 6;
                case TextureGLFormat.RGBA8888: return 4;
                case TextureGLFormat.R16: return 2;
                case TextureGLFormat.RG1616: return 4;
                case TextureGLFormat.RGBA16161616: return 8;
                case TextureGLFormat.R16F: return 2;
                case TextureGLFormat.RG1616F: return 4;
                case TextureGLFormat.RGBA16161616F: return 8;
                case TextureGLFormat.R32F: return 4;
                case TextureGLFormat.RG3232F: return 8;
                case TextureGLFormat.RGB323232F: return 12;
                case TextureGLFormat.RGBA32323232F: return 16;
                case TextureGLFormat.BC6H: return 16;
                case TextureGLFormat.BC7: return 16;
                case TextureGLFormat.IA88: return 2;
                case TextureGLFormat.ETC2: return 8;
                case TextureGLFormat.ETC2_EAC: return 16;
                case TextureGLFormat.BGRA8888: return 4;
                case TextureGLFormat.ATI1N: return 8;
                default: return 1;
            }
        }

        #endregion

        #region TextureOpaque

        class TextureOpaque : ITextureInfo
        {
            internal byte[][] Bytes;
            byte[] ITextureInfo.this[int index]
            {
                get => Bytes[index];
                set => Bytes[index] = value;
            }
            public IDictionary<string, object> Data { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Depth { get; set; }
            public TextureFlags Flags { get; set; }
            public TextureUnityFormat UnityFormat { get; set; }
            public TextureGLFormat GLFormat { get; set; }
            public int NumMipMaps { get; set; }
            public void MoveToData() { }
        }

        public static ITextureInfo DecodeOpaque(byte[] opaque)
        {
            using (var s = new MemoryStream(opaque))
            using (var r = new BinaryReader(s))
            {
                var magicString = r.ReadBytes(4);
                if (!Literal.IMG_.SequenceEqual(magicString))
                    throw new FormatException($"Invalid IMG file magic string: \"{Encoding.ASCII.GetString(magicString)}\".");
                var source = new TextureOpaque
                {
                    UnityFormat = (TextureUnityFormat)r.ReadInt16(),
                    GLFormat = (TextureGLFormat)r.ReadInt16(),
                    Flags = (TextureFlags)r.ReadInt32(),
                    Width = r.ReadInt32(),
                    Height = r.ReadInt32(),
                    Depth = r.ReadInt32(),
                    NumMipMaps = r.ReadByte(),
                };
                source.Bytes = new byte[source.NumMipMaps][];
                for (var i = 0; i < source.NumMipMaps; i++)
                    source.Bytes[i] = r.ReadBytes(r.ReadInt32());
                return source;
            }
        }

        public static byte[] EncodeOpaque(this ITextureInfo source)
        {
            using (var s = new MemoryStream())
            using (var r = new BinaryWriter(s))
            {
                r.Write(Literal.IMG_);
                r.Write((short)source.UnityFormat);
                r.Write((short)source.GLFormat);
                r.Write((int)source.Flags);
                r.Write(source.Width);
                r.Write(source.Height);
                r.Write(source.NumMipMaps);
                for (var i = 0; i < source.NumMipMaps; i++)
                {
                    r.Write(source[0].Length); r.Write(source[0]);
                }
                s.Position = 0;
                return s.ToArray();
            }
        }

        #endregion

        #region Color Shift

        // TODO: Move? Unity?
        public static unsafe ITextureInfo FromABGR555(this ITextureInfo source, int index = 0)
        {
            var W = source.Width; var H = source.Height;
            var pixels = new byte[W * H * 4];
            fixed (byte* pPixels = pixels, pData = source[index])
            {
                var rPixels = (uint*)pPixels;
                var rData = (ushort*)pData;
                for (var i = 0; i < W * H; ++i)
                {
                    var d555 = *rData++;
                    //var a = 0;// (byte)Math.Min(((d555 & 0x8000) >> 15) * 0x1F, byte.MaxValue);
                    //var r = (byte)Math.Min(((d555 & 0x7C00) >> 10) * 8, byte.MaxValue);
                    //var g = (byte)Math.Min(((d555 & 0x03E0) >> 5) * 8, byte.MaxValue);
                    //var b = (byte)Math.Min(((d555 & 0x001F) >> 0) * 8, byte.MaxValue);

                    var r = (byte)Math.Min(((d555 & 0xF800) >> 11) * 8, byte.MaxValue);     // 1111 1000 0000 0000 = F800
                    var g = (byte)Math.Min(((d555 & 0x07C0) >> 6) * 8, byte.MaxValue);      // 0000 0111 1100 0000 = 07C0
                    var b = (byte)Math.Min(((d555 & 0x003E) >> 1) * 8, byte.MaxValue);      // 0000 0000 0011 1110 = 003E
                    var a = (byte)Math.Min((d555 & 0x0001) * 0x1F, byte.MaxValue);          // 0000 0000 0000 0001 = 0001
                    uint color;
                    if (source.UnityFormat == TextureUnityFormat.RGBA32)
                        color =
                            ((uint)(a << 24) & 0xFF000000) |
                            ((uint)(b << 16) & 0x00FF0000) |
                            ((uint)(g << 8) & 0x0000FF00) |
                            ((uint)(r << 0) & 0x000000FF);
                    else if (source.UnityFormat == TextureUnityFormat.ARGB32)
                        color =
                            ((uint)(b << 24) & 0xFF000000) |
                            ((uint)(g << 16) & 0x00FF0000) |
                            ((uint)(r << 8) & 0x0000FF00) |
                            ((uint)(a << 0) & 0x000000FF);
                    else throw new ArgumentOutOfRangeException(nameof(source.UnityFormat), source.UnityFormat.ToString());
                    *rPixels++ = color;
                }
            }
            source[index] = pixels;
            return source;
        }

        // TODO: Move? Unity?
        public static ITextureInfo From8BitPallet(this ITextureInfo source, byte[][] pallet, TextureUnityFormat palletFormat, int index = 0)
        {
            if (source.UnityFormat != palletFormat)
                throw new InvalidOperationException();
            var b = new MemoryStream();
            var d = source[index];
            for (var y = 0; y < d.Length; y++)
                b.Write(pallet[d[y]], 0, 4);
            source[index] = b.ToArray();
            return source;
        }

        #endregion
    }
}