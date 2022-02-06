using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.Threading;

namespace OpenTK
{
    internal class DummyGLControl : IGLControl
    {
        public IGraphicsContext CreateContext(int major, int minor, GraphicsContextFlags flags) => new DummyContext();

        public bool IsIdle => false;

        public IWindowInfo WindowInfo => Utilities.CreateDummyWindowInfo();

        class DummyContext : IGraphicsContext, IDisposable, IGraphicsContextInternal
        {
            static int _instance_count;
            IWindowInfo _current_window;

            public void Dispose() => IsDisposed = true;

            public IntPtr GetAddress(IntPtr function) => IntPtr.Zero;

            public IntPtr GetAddress(string function) => IntPtr.Zero;

            public void LoadAll() { }

            public void MakeCurrent(IWindowInfo window) => _current_window = window;

            public void SwapBuffers() { }

            public void Update(IWindowInfo window) { }

            public bool IsCurrent => _current_window != null;

            public bool IsDisposed { get; private set; }

            public bool VSync
            {
                get => SwapInterval != 0;
                set => SwapInterval = value ? 1 : 0;
            }

            public int SwapInterval { get; set; }

            public GraphicsMode GraphicsMode => GraphicsMode.Default;

            public bool ErrorChecking
            {
                get => false;
                set { }
            }

            public ContextHandle Context { get; } = new ContextHandle(new IntPtr(Interlocked.Increment(ref _instance_count)));

            public IGraphicsContext Implementation => this;
        }
    }
}

