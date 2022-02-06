using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace OpenTK.Platform.MacOS
{
    internal class AglGraphicsMode
    {
        GraphicsMode GetGraphicsModeFromPixelFormat(IntPtr pixelformat)
        {
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_RED_SIZE, out var num);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_GREEN_SIZE, out var num2);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_BLUE_SIZE, out var num3);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_ALPHA_SIZE, out var num4);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_ACCUM_ALPHA_SIZE, out var num8);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_ACCUM_RED_SIZE, out var num5);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_ACCUM_GREEN_SIZE, out var num6);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_ACCUM_BLUE_SIZE, out var num7);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_DEPTH_SIZE, out var num9);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_STENCIL_SIZE, out var num10);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_SAMPLES_ARB, out var num11);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_DOUBLEBUFFER, out var num12);
            Agl.aglDescribePixelFormat(pixelformat, Agl.PixelFormatAttribute.AGL_STEREO, out var num13);
            return new GraphicsMode(new ColorFormat(num, num2, num3, num4), num9, num10, num11, new ColorFormat(num5, num6, num7, num8), num12 + 1, num13 != 0);
        }

        static bool RelaxGraphicsMode(ref ColorFormat color, ref int depth, ref int stencil, ref int samples, ref ColorFormat accum, ref int buffers, ref bool stereo)
        {
            if (accum != 0) { accum = 0; return true; }
            if (buffers > 2) { buffers = 2; return true; }
            if (samples > 0) { samples = Math.Max(samples - 1, 0); return true; }
            if (stereo) { stereo = false; return true; }
            if (stencil != 0) { stencil = 0; return true; }
            if (depth != 0) { depth = 0; return true; }
            if (color != 0x18) { color = 0x18; return true; }
            if (buffers == 0) return false;
            buffers = 0;
            return true;
        }

        public GraphicsMode SelectGraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo, out IntPtr pixelformat)
        {
            while (true)
            {
                pixelformat = SelectPixelFormat(color, depth, stencil, samples, accum, buffers, stereo);
                if ((pixelformat == IntPtr.Zero) && !RelaxGraphicsMode(ref color, ref depth, ref stencil, ref samples, ref accum, ref buffers, ref stereo))
                    throw new GraphicsModeException("Requested GraphicsMode not available, error: " + Agl.GetError());
                if (pixelformat != IntPtr.Zero)
                    return GetGraphicsModeFromPixelFormat(pixelformat);
            }
        }

        IntPtr SelectPixelFormat(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
        {
            var list = new List<int>();
            if (color.BitsPerPixel > 0)
            {
                if (!color.IsIndexed) list.Add(4);
                list.Add(8);
                list.Add(color.Red);
                list.Add(9);
                list.Add(color.Green);
                list.Add(10);
                list.Add(color.Blue);
                list.Add(11);
                list.Add(color.Alpha);
            }
            if (depth > 0) { list.Add(12); list.Add(depth); }
            if (buffers > 1) list.Add(5);
            if (stencil > 0) { list.Add(13); list.Add(stencil); }
            if (accum.BitsPerPixel > 0) { list.Add(0x11); list.Add(accum.Alpha); list.Add(0x10); list.Add(accum.Blue); list.Add(15); list.Add(accum.Green); list.Add(14); list.Add(accum.Red); }
            if (samples > 0) { list.Add(0x37); list.Add(1); list.Add(0x38); list.Add(samples); }
            if (stereo) list.Add(6);
            list.Add(0);
            list.Add(0);
            return Agl.aglChoosePixelFormat(IntPtr.Zero, 0, list.ToArray());
        }
    }
}

