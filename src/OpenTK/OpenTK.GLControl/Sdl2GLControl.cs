using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace OpenTK
{
    internal class Sdl2GLControl : IGLControl
    {
        GraphicsMode _mode;

        static class NativeMethods
        {
            [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern bool SDL_HasEvents(int minType, int maxType);
        }

        public Sdl2GLControl(GraphicsMode mode, IntPtr windowHandle)
        {
            _mode = mode;
            WindowInfo = Utilities.CreateSdl2WindowInfo(windowHandle);
        }

        public IGraphicsContext CreateContext(int major, int minor, GraphicsContextFlags flags) =>
            new GraphicsContext(_mode, WindowInfo, major, minor, flags);

        public bool IsIdle => NativeMethods.SDL_HasEvents(0, 0xffff);

        public IWindowInfo WindowInfo { get; }
    }
}

