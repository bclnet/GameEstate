using OpenTK.Graphics;
using System;
using System.Windows;

namespace OpenTK
{
    internal class GLControlFactory
    {
        public IGLControl CreateGLControl(GraphicsMode mode, IntPtr windowHandle)
        {
            if (mode == null)
                throw new ArgumentNullException(nameof(mode));
            if (Configuration.RunningOnSdl2)
                return new Sdl2GLControl(mode, windowHandle);
            if (Configuration.RunningOnWindows)
                return new WinGLControl(mode, windowHandle);
            //if (Configuration.RunningOnMacOS)
            //    return new CarbonGLControl(mode, windowHandle);
            if (Configuration.RunningOnX11)
                return new X11GLControl(mode, windowHandle);
            throw new PlatformNotSupportedException();
        }
    }
}

