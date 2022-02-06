using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace OpenTK
{
    public class GLControl : HwndHost
    {
        static bool? _isInDesignMode;
        bool _loaded;
        IGraphicsContext _context;
        IGLControl _impl;
        readonly GraphicsMode _format;
        readonly int _major;
        readonly int _minor;
        readonly GraphicsContextFlags _flags;
        bool _resizeEventSuppressed;
        readonly bool _designMode;

        public delegate void DelayUpdate();

        static GLControl()
        {
            // The Stretch & StretchDirection properties are AddOwner'ed from a class which is not
            // base class for Image so the metadata with flags get lost. We need to override them
            // here to make it work again.
            StretchProperty.OverrideMetadata(typeof(GLControl), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));
            StretchDirectionProperty.OverrideMetadata(typeof(GLControl), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure));
        }

        public GLControl() : this(GraphicsMode.Default) { }
        public GLControl(GraphicsMode mode) : this(mode, 1, 0, GraphicsContextFlags.Default) { }
        public GLControl(GraphicsMode mode, int major, int minor, GraphicsContextFlags flags)
        {
            if (mode == null)
                throw new ArgumentNullException(nameof(mode));

            Toolkit.Init(new ToolkitOptions { Backend = PlatformBackend.PreferNative });

            _format = mode;
            _major = major;
            _minor = minor;
            _flags = flags;
            _designMode = IsInDesignMode;
        }

        #region Control

        internal const int WS_CHILD = 0x40000000;
        //internal const int
        //  WS_CHILD = 0x40000000,
        //  WS_VISIBLE = 0x10000000,
        //  LBS_NOTIFY = 0x00000001,
        //  HOST_ID = 0x00000002,
        //  LISTBOX_ID = 0x00000001,
        //  WS_VSCROLL = 0x00200000,
        //  WS_BORDER = 0x00800000;

        //https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/wpf-and-win32-interoperation
        //https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/walkthrough-hosting-a-win32-control-in-wpf
        /// <summary>
        /// When overridden in a derived class, creates the window to be hosted.
        /// </summary>
        /// <param name="hwndParent">The window handle of the parent window.</param>
        /// <returns>
        /// The handle to the child Win32 window to create.
        /// </returns>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            var hwndSource = new HwndSource(new HwndSourceParameters("GL")
            {
                WindowStyle = WS_CHILD,
                ParentWindow = hwndParent.Handle,
            });
            //hwndSource.AddHook(WndHook);
            var hwnd = hwndSource.CreateHandleRef();
            if (!_designMode)
                AttachHandle(hwnd);
            return hwnd;
        }

        /// <summary>
        /// When overridden in a derived class, destroys the hosted window.
        /// </summary>
        /// <param name="hwnd">A structure that contains the window handle.</param>
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            if (!_designMode)
                DestroyHandle(hwnd);
            var hwndSource = (HwndSource)hwnd.Wrapper;
            //hwndSource.RemoveHook(WndHook);
            hwndSource.Dispose();
        }

        bool _mouseEntered;
        int _changing;
        /// <summary>
        /// When overridden in a derived class, accesses the window process (handle) of the hosted child window.
        /// </summary>
        /// <param name="hwnd">The window handle of the hosted window.</param>
        /// <param name="msg">The message to act upon.</param>
        /// <param name="wParam">Information that may be relevant to handling the message. This is typically used to store small pieces of information, such as flags.</param>
        /// <param name="lParam">Information that may be relevant to handling the message. This is typically used to reference an object.</param>
        /// <param name="handled">Whether events resulting should be marked handled.</param>
        /// <returns>
        /// The window handle of the child window.
        /// </returns>
        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_WINDOWPOSCHANGING = 0x0046;
            const int WM_NCHITTEST = 0x0084;
            const int WM_MOUSEWHEEL = 0x020a;
            switch (msg)
            {
                case WM_NCHITTEST:
                    _changing = 5;
                    if (!_mouseEntered)
                    {
                        OnMouseEnter(null);
                        _mouseEntered = true;
                    }
                    break;
                case WM_WINDOWPOSCHANGING:
                    if (_mouseEntered && --_changing < 0)
                    {
                        OnMouseLeave(null);
                        _mouseEntered = false;
                    }
                    break;
                case WM_MOUSEWHEEL:
                    OnMouseWheel(null);
                    break;
                //default: Console.Write($"{msg,5:x}"); break;
            }
            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is in design mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is in design mode; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInDesignMode
        {
            get
            {
                if (_isInDesignMode == null)
                    _isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
                return _isInDesignMode.Value;
            }
        }

        /// <summary>
        /// Gets/Sets the Stretch on this Image.
        /// The Stretch property determines how large the Image will be drawn.
        /// </summary>
        /// <seealso cref="Image.StretchProperty" />
        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        /// <summary>
        /// Gets/Sets the stretch direction of the Viewbox, which determines the restrictions on
        /// scaling that are applied to the content inside the Viewbox.  For instance, this property
        /// can be used to prevent the content from being smaller than its native size or larger than
        /// its native size.
        /// </summary>
        /// <seealso cref="Viewbox.StretchDirectionProperty" />
        public StretchDirection StretchDirection
        {
            get => (StretchDirection)GetValue(StretchDirectionProperty);
            set => SetValue(StretchDirectionProperty, value);
        }

        /// <summary>
        /// DependencyProperty for Stretch property.
        /// </summary>
        /// <seealso cref="Viewbox.Stretch" />
        public static readonly DependencyProperty StretchProperty = Viewbox.StretchProperty.AddOwner(typeof(GLControl));

        /// <summary>
        /// DependencyProperty for StretchDirection property.
        /// </summary>
        /// <seealso cref="Viewbox.Stretch" />
        public static readonly DependencyProperty StretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(typeof(GLControl));

        /// <summary>
        /// The v synchronize property
        /// </summary>
        public static readonly DependencyProperty VSyncProperty = DependencyProperty.Register("VSync", typeof(bool), typeof(GLControl), new PropertyMetadata(true));

        #endregion

        IGLControl Implementation => _impl;

        protected virtual void AttachHandle(HandleRef hwnd)
        {
            if (_designMode || _loaded)
                return;
            _loaded = true;

            if (!(_impl is DummyGLControl))
            {
                _context?.Dispose();
                _impl?.WindowInfo.Dispose();

                if (_designMode)
                {
                    _impl = new DummyGLControl();
                    _context = _impl.CreateContext(_major, _minor, _flags);
                    HasValidContext = false;
                }
                else
                {
                    try
                    {
                        _impl = new GLControlFactory().CreateGLControl(_format, hwnd.Handle);
                        _context = _impl.CreateContext(_major, _minor, _flags);
                        HasValidContext = true;
                    }
                    catch (GraphicsModeException)
                    {
                        _impl = new DummyGLControl();
                        _context = _impl.CreateContext(_major, _minor, _flags);
                        HasValidContext = false;
                    }
                }
                MakeCurrent();

                if (HasValidContext)
                {
                    _context.LoadAll();
                    _context.SwapInterval = (bool)GetValue(VSyncProperty) ? 1 : 0;
                }
            }

            if (_resizeEventSuppressed)
            {
                OnRenderSizeChanged(null);
                _resizeEventSuppressed = false;
            }
        }

        protected virtual void DestroyHandle(HandleRef hwnd)
        {
            if (!(_impl is DummyGLControl))
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }

                if (_impl != null)
                {
                    _impl.WindowInfo.Dispose();
                    _impl = null;
                }
            }
            _loaded = false;
        }

        public void MakeCurrent() => _context?.MakeCurrent(_impl.WindowInfo);

        //protected override void OnRender(DrawingContext dc)
        //{
        //    if (_designMode)
        //    {
        //        dc.DrawRectangle(Brushes.LightGray, new Pen(Brushes.Blue, .5), new Rect(new System.Windows.Point(), RenderSize));
        //        var courier = new FontFamily("Courier New");
        //        var courierTypeface = new Typeface(courier, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
        //        var text = new FormattedText("DESIGN MODE", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, courierTypeface, 24.0, Brushes.Black, 1.25);
        //        dc.DrawText(text, new System.Windows.Point(10, 10));
        //        //dc.DrawRectangle(null, new Pen(Brushes.Red, 1.0), new Rect(10, 10, text.WidthIncludingTrailingWhitespace, text.Height));
        //    }
        //}

        protected override void OnVisualParentChanged(DependencyObject e) { _context?.Update(_impl.WindowInfo); base.OnVisualParentChanged(e); }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (!IsHandleCreated)
                _resizeEventSuppressed = true;
            else
            {
                //if (Configuration.RunningOnMacOS)
                //{
                //    var update = new DelayUpdate(PerformContextUpdate);
                //    base.BeginInvoke((Delegate)update);
                //} else
                _context?.Update(_impl.WindowInfo);
                base.OnRenderSizeChanged(sizeInfo);
            }
        }

        public void PerformContextUpdate() => _context?.Update(_impl.WindowInfo);

        public void Update() => InvalidateVisual();

        public void SwapBuffers() => _context.SwapBuffers();

        //[Conditional("DEBUG")]
        //void ValidateContext(string message) => _ = _context.IsCurrent;

        public bool HasValidContext { get; set; }

        public bool IsIdle => _impl.IsIdle;

        public IGraphicsContext Context => _context;

        [Description("The aspect ratio of the client area of this GLControl.")]
        public float AspectRatio => (float)(ActualWidth / ActualHeight);

        [Description("Indicates whether GLControl updates are synced to the monitor's refresh rate."), Category("Appearance")]
        public bool VSync
        {
            get => !IsHandleCreated ? (bool)GetValue(VSyncProperty) : _context.SwapInterval != 0;
            set
            {
                if (!IsHandleCreated) SetValue(VSyncProperty, value);
                else _context.SwapInterval = value ? 1 : 0;
            }
        }

        public GraphicsMode GraphicsMode => _context.GraphicsMode;

        public IWindowInfo WindowInfo => _impl.WindowInfo;

        public bool IsHandleCreated => ((HwndSource)PresentationSource.FromVisual(this)).Handle != IntPtr.Zero;
    }
}

