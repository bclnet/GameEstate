using OpenTK.Graphics;
using OpenTK.Platform.MacOS.Carbon;
using System;
using System.Collections.Generic;

namespace OpenTK.Platform.MacOS
{
    internal class AglContext : IGraphicsContext, IDisposable, IGraphicsContextInternal
    {
        IWindowInfo _carbonWindow;
        IGraphicsContext _dummyContext;
        readonly GetInt XOffset;
        readonly GetInt YOffset;
        bool _firstSwap = true;

        public AglContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, GetInt xoffset, GetInt yoffset)
        {
            var zero = IntPtr.Zero;
            XOffset = xoffset;
            YOffset = yoffset;
            _carbonWindow = window;
            if (shareContext is AglContext)
                zero = ((AglContext)shareContext).Context.Handle;
            else if (shareContext is GraphicsContext)
                zero = (shareContext != null ? ((IntPtr)(shareContext as IGraphicsContextInternal).Context) : ((IntPtr)((ContextHandle)IntPtr.Zero))).Handle;
            var flag1 = zero == IntPtr.Zero;
            CreateContext(mode, _carbonWindow, zero, true);
        }
        ~AglContext() { Dispose(false); }

        public void Dispose() => Dispose(true);
        void Dispose(bool disposing)
        {
            if (!IsDisposed && Context.Handle != IntPtr.Zero)
            {
                Agl.aglSetCurrentContext(IntPtr.Zero);
                if (Agl.aglDestroyContext(Context.Handle))
                    Context = ContextHandle.Zero;
                else
                {
                    if (disposing)
                        throw new Exception(Agl.ErrorString(Agl.GetError()));
                    IsDisposed = true;
                }
            }
        }

        void AddPixelAttrib(List<int> aglAttributes, Agl.PixelFormatAttribute pixelFormatAttribute) => aglAttributes.Add((int)pixelFormatAttribute);

        void AddPixelAttrib(List<int> aglAttributes, Agl.PixelFormatAttribute pixelFormatAttribute, int value) { aglAttributes.Add((int)pixelFormatAttribute); aglAttributes.Add(value); }

        void CreateContext(GraphicsMode mode, IWindowInfo carbonWindow, IntPtr shareContextRef, bool fullscreen)
        {
            Mode = new AglGraphicsMode().SelectGraphicsMode(mode.ColorFormat, mode.Depth, mode.Stencil, mode.Samples, mode.AccumulatorFormat, mode.Buffers, mode.Stereo, out var ptr);
            MyAGLReportError("aglChoosePixelFormat");
            Context = new ContextHandle(Agl.aglCreateContext(ptr, shareContextRef));
            MyAGLReportError("aglCreateContext");
            Agl.aglDestroyPixelFormat(ptr);
            MyAGLReportError("aglDestroyPixelFormat");
            SetDrawable(carbonWindow);
            SetBufferRect(carbonWindow);
            Update(carbonWindow);
            MakeCurrent(carbonWindow);
            _dummyContext = new GraphicsContext(Context, new GraphicsContext.GetAddressDelegate(GetAddress), () => new ContextHandle(Agl.aglGetCurrentContext()));
        }

        public IntPtr GetAddress(IntPtr function) => NS.GetAddress(function);

        public IntPtr GetAddress(string function) => NS.GetAddress(function);

        static IntPtr GetWindowPortForWindowInfo(IWindowInfo carbonWindow) => API.GetWindowPort(API.GetControlOwner(carbonWindow.Handle));

        public void LoadAll() => _dummyContext.LoadAll();

        public void MakeCurrent(IWindowInfo window)
        {
            if (!Agl.aglSetCurrentContext(Context.Handle))
                MyAGLReportError("aglSetCurrentContext");
        }

        void MyAGLReportError(string function)
        {
            var error = Agl.GetError();
            if (error != Agl.AglError.NoError)
                throw new Exception($"AGL Error from function {function}: {error}  {Agl.ErrorString(error)}");
        }

        void SetBufferRect(IWindowInfo carbonWindow)
        {
            var controlBounds = API.GetControlBounds(carbonWindow.Handle);
            var @params = new[] { XOffset == null ? controlBounds.X : controlBounds.X + XOffset(), YOffset == null ? controlBounds.Y : controlBounds.Y + YOffset(), controlBounds.Width, controlBounds.Height };
            Agl.aglSetInteger(Context.Handle, Agl.ParameterNames.AGL_BUFFER_RECT, @params);
            MyAGLReportError("aglSetInteger");
            Agl.aglEnable(Context.Handle, Agl.ParameterNames.AGL_BUFFER_RECT);
            MyAGLReportError("aglEnable");
        }

        void SetDrawable(IWindowInfo carbonWindow)
        {
            var windowPortForWindowInfo = GetWindowPortForWindowInfo(carbonWindow);
            Agl.aglSetDrawable(Context.Handle, windowPortForWindowInfo);
            MyAGLReportError("aglSetDrawable");
        }

        public void SwapBuffers()
        {
            if (_firstSwap)
            {
                _firstSwap = false;
                SetDrawable(_carbonWindow);
                Update(_carbonWindow);
            }
            Agl.aglSwapBuffers(Context.Handle);
            MyAGLReportError("aglSwapBuffers");
        }

        public void Update(IWindowInfo window)
        {
            SetDrawable(window);
            SetBufferRect(window);
            Agl.aglUpdateContext(Context.Handle);
        }

        public bool IsCurrent => Context.Handle == Agl.aglGetCurrentContext();

        public int SwapInterval
        {
            get
            {
                if (Agl.aglGetInteger(Context.Handle, Agl.ParameterNames.AGL_SWAP_INTERVAL, out var param))
                    return param;
                MyAGLReportError("aglGetInteger");
                return 0;
            }
            set
            {
                if (!Agl.aglSetInteger(Context.Handle, Agl.ParameterNames.AGL_SWAP_INTERVAL, ref value))
                    MyAGLReportError("aglSetInteger");
            }
        }

        public GraphicsMode Mode { get; set; }

        public bool IsDisposed { get; private set; }

        public bool VSync
        {
            get => SwapInterval != 0;
            set => SwapInterval = value ? 1 : 0;
        }

        public GraphicsMode GraphicsMode => Mode;

        public bool ErrorChecking { get; set; }

        public IGraphicsContext Implementation => this;

        public ContextHandle Context { get; private set; }
    }
}

