using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenTK
{
    internal class WinGLControl : IGLControl
    {
        MSG _msg;
        GraphicsMode _mode;

        [StructLayout(LayoutKind.Sequential)]
        struct MSG
        {
            public IntPtr HWnd;
            public uint Message;
            public IntPtr WParam;
            public IntPtr LParam;
            public uint Time;
            public POINT Point;
            public override string ToString() => $"msg=0x{((int)Message):x} ({Message}) hwnd=0x{HWnd.ToInt32():x} wparam=0x{WParam.ToInt32():x} lparam=0x{LParam.ToInt32():x} pt=0x{Point:x}";
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y) { X = x; Y = y; }
            public System.Drawing.Point ToPoint() => new System.Drawing.Point(X, Y);
            public override string ToString() => $"Point ({X}, {Y})";
        }

        public WinGLControl(GraphicsMode mode, IntPtr windowHandle)
        {
            _mode = mode;
            WindowInfo = Utilities.CreateWindowsWindowInfo(windowHandle); //((HwndSource)HwndSource.FromVisual(control)).Handle
        }

        public IGraphicsContext CreateContext(int major, int minor, GraphicsContextFlags flags) => new GraphicsContext(_mode, WindowInfo, major, minor, flags);

        [SuppressUnmanagedCodeSecurity, DllImport("User32.dll")] static extern bool PeekMessage(ref MSG msg, IntPtr hWnd, int messageFilterMin, int messageFilterMax, int flags);

        public bool IsIdle => !PeekMessage(ref _msg, IntPtr.Zero, 0, 0, 0);

        public IWindowInfo WindowInfo { get; }
    }
}

