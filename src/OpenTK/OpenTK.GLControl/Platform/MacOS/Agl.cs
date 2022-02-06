using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS
{
    internal static class Agl
    {
        const string agl = "/System/Library/Frameworks/AGL.framework/Versions/Current/AGL";
        const int AGL_VERSION_2_0 = 1;

        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglDestroyContext")]
        static extern byte _aglDestroyContext(IntPtr ctx);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglErrorString")]
        static extern IntPtr _aglErrorString(AglError code);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglSetCurrentContext")]
        static extern byte _aglSetCurrentContext(IntPtr ctx);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglSetDrawable")]
        static extern byte _aglSetDrawable(IntPtr ctx, IntPtr draw);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglSetFullScreen")]
        static extern byte _aglSetFullScreen(IntPtr ctx, int width, int height, int freq, int device);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern IntPtr aglChoosePixelFormat(ref IntPtr gdevs, int ndev, int[] attribs);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern IntPtr aglChoosePixelFormat(IntPtr gdevs, int ndev, int[] attribs);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern byte aglConfigure(uint pname, uint param);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern byte aglCopyContext(IntPtr src, IntPtr dst, uint mask);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern IntPtr aglCreateContext(IntPtr pix, IntPtr share);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern unsafe byte aglCreatePBuffer(int width, int height, uint target, uint internalFormat, long max_level, IntPtr* pbuffer);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern unsafe byte aglDescribePBuffer(IntPtr pbuffer, int* width, int* height, uint* target, uint* internalFormat, int* max_level);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern bool aglDescribePixelFormat(IntPtr pix, PixelFormatAttribute attrib, out int value);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern byte aglDescribeRenderer(IntPtr rend, int prop, out int value);
        internal static bool aglDestroyContext(IntPtr context) => _aglDestroyContext(context) != 0;

        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern byte aglDestroyPBuffer(IntPtr pbuffer);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern void aglDestroyPixelFormat(IntPtr pix);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern void aglDestroyRendererInfo(IntPtr rend);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern bool aglDisable(IntPtr ctx, ParameterNames pname);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern bool aglEnable(IntPtr ctx, ParameterNames pname);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern unsafe byte aglGetCGLContext(IntPtr ctx, void** cgl_ctx);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern unsafe byte aglGetCGLPixelFormat(IntPtr pix, void** cgl_pix);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern IntPtr aglGetCurrentContext();
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern IntPtr aglGetDrawable(IntPtr ctx);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern bool aglGetInteger(IntPtr ctx, ParameterNames pname, out int param);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern unsafe byte aglGetPBuffer(IntPtr ctx, IntPtr* pbuffer, int* face, int* level, int* screen);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern unsafe void aglGetVersion(int* major, int* minor);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern int aglGetVirtualScreen(IntPtr ctx);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern bool aglIsEnabled(IntPtr ctx, uint pname);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern IntPtr aglNextPixelFormat(IntPtr pix);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern IntPtr aglNextRendererInfo(IntPtr rend);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern IntPtr aglQueryRendererInfo(IntPtr[] gdevs, int ndev);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern void aglResetLibrary();
        internal static bool aglSetCurrentContext(IntPtr context) =>
            (_aglSetCurrentContext(context) != 0);

        internal static void aglSetDrawable(IntPtr ctx, IntPtr draw)
        {
            if (_aglSetDrawable(ctx, draw) == 0)
                throw new Exception(ErrorString(GetError()));
        }

        internal static void aglSetFullScreen(IntPtr ctx, int width, int height, int freq, int device)
        {
            if (_aglSetFullScreen(ctx, width, height, freq, device) == 0)
                throw new Exception(ErrorString(GetError()));
        }

        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern bool aglSetInteger(IntPtr ctx, ParameterNames pname, ref int param);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern bool aglSetInteger(IntPtr ctx, ParameterNames pname, int[] @params);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern byte aglSetOffScreen(IntPtr ctx, int width, int height, int rowbytes, IntPtr baseaddr);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern byte aglSetPBuffer(IntPtr ctx, IntPtr pbuffer, int face, int level, int screen);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern byte aglSetVirtualScreen(IntPtr ctx, int screen);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern void aglSurfaceTexture(IntPtr context, uint target, uint internalformat, IntPtr surfacecontext);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern void aglSwapBuffers(IntPtr ctx);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern byte aglTexImagePBuffer(IntPtr ctx, IntPtr pbuffer, int source);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        internal static extern byte aglUpdateContext(IntPtr ctx);
        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL")]
        static extern byte aglUseFont(IntPtr ctx, int fontID, int face, int size, int first, int count, int @base);
        internal static string ErrorString(AglError code) => Marshal.PtrToStringAnsi(_aglErrorString(code));

        [DllImport("/System/Library/Frameworks/AGL.framework/Versions/Current/AGL", EntryPoint = "aglGetError")]
        internal static extern AglError GetError();

        internal enum AglError
        {
            NoError = 0,
            BadAttribute = 0x2710,
            BadProperty = 0x2711,
            BadPixelFormat = 0x2712,
            BadRendererInfo = 0x2713,
            BadContext = 0x2714,
            BadDrawable = 0x2715,
            BadGraphicsDevice = 0x2716,
            BadState = 0x2717,
            BadValue = 0x2718,
            BadMatch = 0x2719,
            BadEnum = 0x271a,
            BadOffscreen = 0x271b,
            BadFullscreen = 0x271c,
            BadWindow = 0x271d,
            BadPointer = 0x271e,
            BadModule = 0x271f,
            BadAlloc = 0x2720,
            BadConnection = 0x2721
        }

        internal enum BitDepths
        {
            AGL_0_BIT = 1,
            AGL_1_BIT = 2,
            AGL_2_BIT = 4,
            AGL_3_BIT = 8,
            AGL_4_BIT = 0x10,
            AGL_5_BIT = 0x20,
            AGL_6_BIT = 0x40,
            AGL_8_BIT = 0x80,
            AGL_10_BIT = 0x100,
            AGL_12_BIT = 0x200,
            AGL_16_BIT = 0x400,
            AGL_24_BIT = 0x800,
            AGL_32_BIT = 0x1000,
            AGL_48_BIT = 0x2000,
            AGL_64_BIT = 0x4000,
            AGL_96_BIT = 0x8000,
            AGL_128_BIT = 0x10000
        }

        internal enum BufferModes
        {
            AGL_MONOSCOPIC_BIT = 1,
            AGL_STEREOSCOPIC_BIT = 2,
            AGL_SINGLEBUFFER_BIT = 4,
            AGL_DOUBLEBUFFER_BIT = 8
        }

        internal enum ColorModes
        {
            AGL_RGB8_BIT = 1,
            AGL_RGB8_A8_BIT = 2,
            AGL_BGR233_BIT = 4,
            AGL_BGR233_A8_BIT = 8,
            AGL_RGB332_BIT = 0x10,
            AGL_RGB332_A8_BIT = 0x20,
            AGL_RGB444_BIT = 0x40,
            AGL_ARGB4444_BIT = 0x80,
            AGL_RGB444_A8_BIT = 0x100,
            AGL_RGB555_BIT = 0x200,
            AGL_ARGB1555_BIT = 0x400,
            AGL_RGB555_A8_BIT = 0x800,
            AGL_RGB565_BIT = 0x1000,
            AGL_RGB565_A8_BIT = 0x2000,
            AGL_RGB888_BIT = 0x4000,
            AGL_ARGB8888_BIT = 0x8000,
            AGL_RGB888_A8_BIT = 0x10000,
            AGL_RGB101010_BIT = 0x20000,
            AGL_ARGB2101010_BIT = 0x40000,
            AGL_RGB101010_A8_BIT = 0x80000,
            AGL_RGB121212_BIT = 0x100000,
            AGL_ARGB12121212_BIT = 0x200000,
            AGL_RGB161616_BIT = 0x400000,
            AGL_ARGB16161616_BIT = 0x800000,
            AGL_INDEX8_BIT = 0x20000000,
            AGL_INDEX16_BIT = 0x40000000,
            AGL_RGBFLOAT64_BIT = 0x1000000,
            AGL_RGBAFLOAT64_BIT = 0x2000000,
            AGL_RGBFLOAT128_BIT = 0x4000000,
            AGL_RGBAFLOAT128_BIT = 0x8000000,
            AGL_RGBFLOAT256_BIT = 0x10000000,
            AGL_RGBAFLOAT256_BIT = 0x20000000
        }

        internal enum ExtendedAttribute
        {
            AGL_PIXEL_SIZE = 50,
            AGL_MINIMUM_POLICY = 0x33,
            AGL_MAXIMUM_POLICY = 0x34,
            AGL_OFFSCREEN = 0x35,
            AGL_FULLSCREEN = 0x36,
            AGL_SAMPLE_BUFFERS_ARB = 0x37,
            AGL_SAMPLES_ARB = 0x38,
            AGL_AUX_DEPTH_STENCIL = 0x39,
            AGL_COLOR_FLOAT = 0x3a,
            AGL_MULTISAMPLE = 0x3b,
            AGL_SUPERSAMPLE = 60,
            AGL_SAMPLE_ALPHA = 0x3d
        }

        internal enum OptionName
        {
            AGL_FORMAT_CACHE_SIZE = 0x1f5,
            AGL_CLEAR_FORMAT_CACHE = 0x1f6,
            AGL_RETAIN_RENDERERS = 0x1f7
        }

        internal enum ParameterNames
        {
            AGL_SWAP_RECT = 200,
            AGL_BUFFER_RECT = 0xca,
            AGL_SWAP_LIMIT = 0xcb,
            AGL_COLORMAP_TRACKING = 210,
            AGL_COLORMAP_ENTRY = 0xd4,
            AGL_RASTERIZATION = 220,
            AGL_SWAP_INTERVAL = 0xde,
            AGL_STATE_VALIDATION = 230,
            AGL_BUFFER_NAME = 0xe7,
            AGL_ORDER_CONTEXT_TO_FRONT = 0xe8,
            AGL_CONTEXT_SURFACE_ID = 0xe9,
            AGL_CONTEXT_DISPLAY_ID = 0xea,
            AGL_SURFACE_ORDER = 0xeb,
            AGL_SURFACE_OPACITY = 0xec,
            AGL_CLIP_REGION = 0xfe,
            AGL_FS_CAPTURE_SINGLE = 0xff,
            AGL_SURFACE_BACKING_SIZE = 0x130,
            AGL_ENABLE_SURFACE_BACKING_SIZE = 0x131,
            AGL_SURFACE_VOLATILE = 0x132
        }

        internal enum PixelFormatAttribute
        {
            AGL_NONE = 0,
            AGL_ALL_RENDERERS = 1,
            AGL_BUFFER_SIZE = 2,
            AGL_LEVEL = 3,
            AGL_RGBA = 4,
            AGL_DOUBLEBUFFER = 5,
            AGL_STEREO = 6,
            AGL_AUX_BUFFERS = 7,
            AGL_RED_SIZE = 8,
            AGL_GREEN_SIZE = 9,
            AGL_BLUE_SIZE = 10,
            AGL_ALPHA_SIZE = 11,
            AGL_DEPTH_SIZE = 12,
            AGL_STENCIL_SIZE = 13,
            AGL_ACCUM_RED_SIZE = 14,
            AGL_ACCUM_GREEN_SIZE = 15,
            AGL_ACCUM_BLUE_SIZE = 0x10,
            AGL_ACCUM_ALPHA_SIZE = 0x11,
            AGL_PIXEL_SIZE = 50,
            AGL_MINIMUM_POLICY = 0x33,
            AGL_MAXIMUM_POLICY = 0x34,
            AGL_OFFSCREEN = 0x35,
            AGL_FULLSCREEN = 0x36,
            AGL_SAMPLE_BUFFERS_ARB = 0x37,
            AGL_SAMPLES_ARB = 0x38,
            AGL_AUX_DEPTH_STENCIL = 0x39,
            AGL_COLOR_FLOAT = 0x3a,
            AGL_MULTISAMPLE = 0x3b,
            AGL_SUPERSAMPLE = 60,
            AGL_SAMPLE_ALPHA = 0x3d
        }

        internal enum RendererManagement
        {
            AGL_RENDERER_ID = 70,
            AGL_SINGLE_RENDERER = 0x47,
            AGL_NO_RECOVERY = 0x48,
            AGL_ACCELERATED = 0x49,
            AGL_CLOSEST_POLICY = 0x4a,
            AGL_ROBUST = 0x4b,
            AGL_BACKING_STORE = 0x4c,
            AGL_MP_SAFE = 0x4e,
            AGL_WINDOW = 80,
            AGL_MULTISCREEN = 0x51,
            AGL_VIRTUAL_SCREEN = 0x52,
            AGL_COMPLIANT = 0x53,
            AGL_PBUFFER = 90,
            AGL_REMOTE_PBUFFER = 0x5b
        }

        internal enum RendererProperties
        {
            AGL_BUFFER_MODES = 100,
            AGL_MIN_LEVEL = 0x65,
            AGL_MAX_LEVEL = 0x66,
            AGL_COLOR_MODES = 0x67,
            AGL_ACCUM_MODES = 0x68,
            AGL_DEPTH_MODES = 0x69,
            AGL_STENCIL_MODES = 0x6a,
            AGL_MAX_AUX_BUFFERS = 0x6b,
            AGL_VIDEO_MEMORY = 120,
            AGL_TEXTURE_MEMORY = 0x79,
            AGL_RENDERER_COUNT = 0x80
        }
    }
}

