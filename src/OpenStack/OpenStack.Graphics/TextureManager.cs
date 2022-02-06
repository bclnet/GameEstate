using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace OpenStack.Graphics
{
    public interface ITextureManager<Texture>
    {
        Texture DefaultTexture { get; }
        Texture LoadTexture(object key, out IDictionary<string, object> data);
        void PreloadTexture(string path);
        public Texture BuildSolidTexture(int width, int height, params float[] rgba);
        public Texture BuildNormalMap(Texture source, float strength);
    }

    public enum TextureFlags : int
    {
#pragma warning disable 1591
        SUGGEST_CLAMPS = 0x00000001,
        SUGGEST_CLAMPT = 0x00000002,
        SUGGEST_CLAMPU = 0x00000004,
        NO_LOD = 0x00000008,
        CUBE_TEXTURE = 0x00000010,
        VOLUME_TEXTURE = 0x00000020,
        TEXTURE_ARRAY = 0x00000040,
#pragma warning restore 1591
    }

    public enum TextureUnityFormat : short
    {
        ATC_RGB4 = -127,
        ATC_RGBA8 = -127,
        PVRTC_2BPP_RGB = -127,
        PVRTC_2BPP_RGBA = -127,
        PVRTC_4BPP_RGB = -127,
        PVRTC_4BPP_RGBA = -127,
        Alpha8 = 1,
        ARGB4444 = 2,
        RGB24 = 3,
        RGBA32 = 4,
        ARGB32 = 5,
        RGB565 = 7,
        R16 = 9,
        DXT1 = 10,
        DXT5 = 12,
        RGBA4444 = 13,
        BGRA32 = 14,
        RHalf = 15,
        RGHalf = 16,
        RGBAHalf = 17,
        RFloat = 18,
        RGFloat = 19,
        RGBAFloat = 20,
        YUY2 = 21,
        RGB9e5Float = 22,
        BC6H = 24,
        BC7 = 25,
        BC4 = 26,
        BC5 = 27,
        DXT1Crunched = 28,
        DXT5Crunched = 29,
        PVRTC_RGB2 = 30,
        PVRTC_RGBA2 = 31,
        PVRTC_RGB4 = 32,
        PVRTC_RGBA4 = 33,
        ETC_RGB4 = 34,
        EAC_R = 41,
        EAC_R_SIGNED = 42,
        EAC_RG = 43,
        EAC_RG_SIGNED = 44,
        ETC2_RGB = 45,
        ETC2_RGBA1 = 46,
        ETC2_RGBA8 = 47,
        ASTC_4x4 = 48,
        ASTC_RGB_4x4 = 48,
        ASTC_5x5 = 49,
        ASTC_RGB_5x5 = 49,
        ASTC_6x6 = 50,
        ASTC_RGB_6x6 = 50,
        ASTC_8x8 = 51,
        ASTC_RGB_8x8 = 51,
        ASTC_10x10 = 52,
        ASTC_RGB_10x10 = 52,
        ASTC_12x12 = 53,
        ASTC_RGB_12x12 = 53,
        ASTC_RGBA_4x4 = 54,
        ASTC_RGBA_5x5 = 55,
        ASTC_RGBA_6x6 = 56,
        ASTC_RGBA_8x8 = 57,
        ASTC_RGBA_10x10 = 58,
        ASTC_RGBA_12x12 = 59,
        ETC_RGB4_3DS = 60,
        ETC_RGBA8_3DS = 61,
        RG16 = 62,
        R8 = 63,
        ETC_RGB4Crunched = 64,
        ETC2_RGBA8Crunched = 65,
        ASTC_HDR_4x4 = 66,
        ASTC_HDR_5x5 = 67,
        ASTC_HDR_6x6 = 68,
        ASTC_HDR_8x8 = 69,
        ASTC_HDR_10x10 = 70,
        ASTC_HDR_12x12 = 71
    }

    public enum TextureGLFormat : short
    {
#pragma warning disable 1591
        UNKNOWN = 0,
        DXT1 = 1,
        DXT5 = 2,
        I8 = 3,
        RGBA8888 = 4,
        R16 = 5,
        RG1616 = 6,
        RGBA16161616 = 7,
        R16F = 8,
        RG1616F = 9,
        RGBA16161616F = 10,
        R32F = 11,
        RG3232F = 12,
        RGB323232F = 13,
        RGBA32323232F = 14,
        JPEG_RGBA8888 = 15,
        PNG_RGBA8888 = 16,
        JPEG_DXT5 = 17,
        PNG_DXT5 = 18,
        BC6H = 19,
        BC7 = 20,
        ATI2N = 21,
        IA88 = 22,
        ETC2 = 23,
        ETC2_EAC = 24,
        R11_EAC = 25,
        RG11_EAC = 26,
        ATI1N = 27,
        BGRA8888 = 28,
#pragma warning restore 1591
    }

    /// <summary>
    /// ITextureInfo
    /// </summary>
    public interface ITextureInfo
    {
        byte[] this[int index] { get; set; }
        IDictionary<string, object> Data { get; }
        int Width { get; }
        int Height { get; }
        int Depth { get; }
        TextureFlags Flags { get; }
        TextureUnityFormat UnityFormat { get; }
        TextureGLFormat GLFormat { get; }
        int NumMipMaps { get; }
        void MoveToData();
    }
    
    //public interface ITextureInfoLoad1 : ITextureInfo { }

    /// <summary>
    /// Stores information about a texture.
    /// </summary>
    public class TextureInfo : Dictionary<string, object> //, IGetExplorerInfo
    {
        public int Width, Height, Depth;
        public TextureUnityFormat UnityFormat;
        public TextureGLFormat GLFormat;
        public TextureFlags Flags;
        public bool HasMipmaps;
        public ushort Mipmaps;
        public byte BytesPerPixel;
        public byte[] Data;
        public Action Decompress;
        public int[] CompressedSizeForMipLevel;

        //public TextureInfo() { }
        //public TextureInfo(int width, int height, ushort mipmaps, byte[] data)
        //{
        //    Width = width;
        //    Height = height;
        //    Mipmaps = mipmaps;
        //    Data = data;
        //}

        //List<ExplorerInfoNode> IGetExplorerInfo.GetInfoNodes(ExplorerManager resource, FileMetadata file, object tag)
        //    => new List<ExplorerInfoNode> {
        //    new ExplorerInfoNode(null, new ExplorerContentTab { Type = "Texture", Value = this }),
        //    new ExplorerInfoNode("Texture", items: new List<ExplorerInfoNode> {
        //        new ExplorerInfoNode($"Width: {Width}"),
        //        new ExplorerInfoNode($"Height: {Height}"),
        //        new ExplorerInfoNode($"GLFormat: {GLFormat}"),
        //        new ExplorerInfoNode($"Mipmaps: {Mipmaps}"),
        //    }),
        //};

        public BinaryReader GetDecompressedBuffer(int offset)
           => throw new NotImplementedException();

        //BinaryReader GetDecompressedBuffer()
        //{
        //    if (!IsActuallyCompressedMips)
        //        return Reader;
        //    var outStream = new MemoryStream(GetDecompressedTextureAtMipLevel(MipmapLevelToExtract), false);
        //    return new BinaryReader(outStream); // TODO: dispose
        //}

        //public byte[] GetDecompressedTextureAtMipLevel(int mipLevel)
        //{
        //    var uncompressedSize = CalculateBufferSizeForMipLevel(mipLevel);
        //    if (!IsActuallyCompressedMips)
        //        return Reader.ReadBytes(uncompressedSize);
        //    var compressedSize = CompressedMips[mipLevel];
        //    if (compressedSize >= uncompressedSize)
        //        return Reader.ReadBytes(uncompressedSize);
        //    var input = Reader.ReadBytes(compressedSize);
        //    var output = new Span<byte>(new byte[uncompressedSize]);
        //    LZ4Codec.Decode(input, output);
        //    return output.ToArray();
        //}

        #region MipMap

        public int GetDataOffsetForMip(int mipLevel)
        {
            if (Mipmaps < 2) return 0;

            var offset = 0;
            for (var j = Mipmaps - 1; j > mipLevel; j--)
                offset += CompressedSizeForMipLevel != null
                    ? CompressedSizeForMipLevel[j]
                    : GetMipmapTrueDataSize(Width, Height, Depth, GLFormat, j) * (Flags.HasFlag(TextureFlags.CUBE_TEXTURE) ? 6 : 1);
            return offset;
        }

        public object GetDataSpanForMip(int mipLevel)
        {
            return null;
            //var offset = GetDataOffsetForMip(mipLevel);
            //var dataSize = GetMipmapDataSize(Width, Height, Depth, GLFormat, mipLevel);
            //if (CompressedSizeForMipLevel == null)
            //    return new Span<byte>(Data, 10, 10);
            //var compressedSize = CompressedSizeForMipLevel[mipLevel];
            //if (compressedSize >= dataSize)
            //    return Reader.ReadBytes(dataSize);
            //var input = Reader.ReadBytes(compressedSize);
            //var output = new Span<byte>(new byte[dataSize]);
            //LZ4Codec.Decode(input, output);
            //return output.ToArray();
        }

        public static int GetMipmapCount(int width, int height)
        {
            Debug.Assert(width > 0 && height > 0);
            var longerLength = Math.Max(width, height);
            var mipMapCount = 0;
            var currentLongerLength = longerLength;
            while (currentLongerLength > 0) { mipMapCount++; currentLongerLength /= 2; }
            return mipMapCount;
        }

        public static int GetMipmapDataSize(int width, int height, int bytesPerPixel)
        {
            Debug.Assert(width > 0 && height > 0 && bytesPerPixel > 0);
            var dataSize = 0;
            var currentWidth = width;
            var currentHeight = height;
            while (true)
            {
                dataSize += currentWidth * currentHeight * bytesPerPixel;
                if (currentWidth == 1 && currentHeight == 1) break;
                currentWidth = currentWidth > 1 ? (currentWidth / 2) : currentWidth;
                currentHeight = currentHeight > 1 ? (currentHeight / 2) : currentHeight;
            }
            return dataSize;
        }

        public static int GetMipmapTrueDataSize(int width, int height, int depth, TextureGLFormat format, int mipLevel)
        {
            var bytesPerPixel = format.GetBlockSize();
            var currentWidth = width >> mipLevel;
            var currentHeight = height >> mipLevel;
            var currentDepth = depth >> mipLevel;
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

        // TODO: Improve algorithm for images with odd dimensions.
        public static void Downscale4Component32BitPixelsX2(byte[] srcBytes, int srcStartIndex, int srcRowCount, int srcColumnCount, byte[] dstBytes, int dstStartIndex)
        {
            var bytesPerPixel = 4;
            var componentCount = 4;
            Debug.Assert(srcStartIndex >= 0 && srcRowCount >= 0 && srcColumnCount >= 0 && (srcStartIndex + (bytesPerPixel * srcRowCount * srcColumnCount)) <= srcBytes.Length);
            var dstRowCount = srcRowCount / 2;
            var dstColumnCount = srcColumnCount / 2;
            Debug.Assert(dstStartIndex >= 0 && (dstStartIndex + (bytesPerPixel * dstRowCount * dstColumnCount)) <= dstBytes.Length);
            for (var dstRowIndex = 0; dstRowIndex < dstRowCount; dstRowIndex++)
                for (var dstColumnIndex = 0; dstColumnIndex < dstColumnCount; dstColumnIndex++)
                {
                    var srcRowIndex0 = 2 * dstRowIndex;
                    var srcColumnIndex0 = 2 * dstColumnIndex;
                    var srcPixel0Index = (srcColumnCount * srcRowIndex0) + srcColumnIndex0;

                    var srcPixelStartIndices = new int[4];
                    srcPixelStartIndices[0] = srcStartIndex + (bytesPerPixel * srcPixel0Index); // top-left
                    srcPixelStartIndices[1] = srcPixelStartIndices[0] + bytesPerPixel; // top-right
                    srcPixelStartIndices[2] = srcPixelStartIndices[0] + (bytesPerPixel * srcColumnCount); // bottom-left
                    srcPixelStartIndices[3] = srcPixelStartIndices[2] + bytesPerPixel; // bottom-right

                    var dstPixelIndex = (dstColumnCount * dstRowIndex) + dstColumnIndex;
                    var dstPixelStartIndex = dstStartIndex + (bytesPerPixel * dstPixelIndex);
                    for (var componentIndex = 0; componentIndex < componentCount; componentIndex++)
                    {
                        var averageComponent = 0F;
                        for (var srcPixelIndex = 0; srcPixelIndex < srcPixelStartIndices.Length; srcPixelIndex++) averageComponent += srcBytes[srcPixelStartIndices[srcPixelIndex] + componentIndex];
                        averageComponent /= srcPixelStartIndices.Length;
                        dstBytes[dstPixelStartIndex + componentIndex] = (byte)Math.Round(averageComponent);
                    }
                }
        }

        public byte[] GetTexture(int offset)
            => throw new NotImplementedException();

        public byte[] GetDecompressedTextureAtMipLevel(int offset, int v)
            => throw new NotImplementedException();

        internal static int GetMipmapDataSize(int dwWidth, int dwHeight, int v, object bytesPerPixel)
            => throw new NotImplementedException();

        #endregion
    }
}