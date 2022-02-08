using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenTK
{
    internal class X11GLControl : IGLControl
    {
        GraphicsMode _mode;
        IntPtr _display;
        IntPtr _rootWindow;
        Type _xplatui = Type.GetType("System.Windows.Forms.XplatUIX11, System.Windows.Forms");

        [StructLayout(LayoutKind.Sequential)]
        struct XVisualInfo
        {
            public IntPtr Visual;
            public IntPtr VisualID;
            public int Screen;
            public int Depth;
            public int Class;
            public long RedMask;
            public long GreenMask;
            public long blueMask;
            public int ColormapSize;
            public int BitsPerRgb;
            public override string ToString() => $"id ({VisualID}), screen ({Screen}), depth ({Depth}), class ({Class})";
        }

        internal X11GLControl(GraphicsMode mode, IntPtr windowHandle)
        {
            if (mode == null) throw new ArgumentNullException(nameof(mode));

            _mode = new GraphicsMode(new ColorFormat(mode.ColorFormat.Red, mode.ColorFormat.Green, mode.ColorFormat.Blue, 0), mode.Depth, mode.Stencil, mode.Samples, mode.AccumulatorFormat, mode.Buffers, mode.Stereo);
            if (_xplatui == null) throw new PlatformNotSupportedException("System.Windows.Forms.XplatUIX11 missing. Unsupported platform or Mono runtime version, aborting.");
            _display = (IntPtr)GetStaticFieldValue(_xplatui, "DisplayHandle");
            _rootWindow = (IntPtr)GetStaticFieldValue(_xplatui, "RootWindow");
            var staticFieldValue = (int)GetStaticFieldValue(_xplatui, "ScreenNo");
            WindowInfo = Utilities.CreateX11WindowInfo(_display, staticFieldValue, windowHandle, _rootWindow, IntPtr.Zero);
        }

        public IGraphicsContext CreateContext(int major, int minor, GraphicsContextFlags flags)
        {
            var context = new GraphicsContext(_mode, WindowInfo, major, minor, flags);
            _mode = context.GraphicsMode;
            var template = new XVisualInfo { VisualID = _mode.Index.Value };
            template = (XVisualInfo)Marshal.PtrToStructure(XGetVisualInfo(_display, 1, ref template, out var num), typeof(XVisualInfo));
            SetStaticFieldValue(_xplatui, "CustomVisual", template.Visual);
            SetStaticFieldValue(_xplatui, "CustomColormap", XCreateColormap(_display, _rootWindow, template.Visual, 0));
            return context;
        }

        static object GetStaticFieldValue(Type type, string fieldName) => type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

        static void SetStaticFieldValue(Type type, string fieldName, object value) => type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, value);

        [DllImport("libX11")] static extern IntPtr XCreateColormap(IntPtr display, IntPtr window, IntPtr visual, int alloc);
        static IntPtr XGetVisualInfo(IntPtr display, int vinfo_mask, ref XVisualInfo template, out int nitems) => XGetVisualInfoInternal(display, (IntPtr)vinfo_mask, ref template, out nitems);

        [DllImport("libX11", EntryPoint = "XGetVisualInfo")] static extern IntPtr XGetVisualInfoInternal(IntPtr display, IntPtr vinfo_mask, ref XVisualInfo template, out int nitems);
        [DllImport("libX11")] static extern int XPending(IntPtr diplay);

        public bool IsIdle => XPending(_display) == 0;

        public IWindowInfo WindowInfo { get; }
    }
}

