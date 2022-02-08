using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Platform.MacOS;
using System.Windows.Controls;

namespace OpenTK
{
    internal class CarbonGLControl : IGLControl
    {
        GraphicsMode _mode;
        Control _control;
        bool _lastIsIdle;

        internal CarbonGLControl(GraphicsMode mode, IntPtr windowHandle)
        {
            _mode = mode;
            _control = owner;
            WindowInfo = Utilities.CreateMacOSCarbonWindowInfo(_control.Handle, false, true);
        }

        public IGraphicsContext CreateContext(int major, int minor, GraphicsContextFlags flags) => new AglContext(_mode, WindowInfo, GraphicsContext.CurrentContext, new GetInt(GetXOffset), new GetInt(GetYOffset));

        int GetXOffset() => _control.Location.X;

        int GetYOffset()
        {
            if (_control.TopLevelControl == null) return _control.Location.Y;
            var point = _control.PointToScreen(_control.Location);
            var point2 = _control.TopLevelControl.PointToScreen(Point.Empty);
            var num = point.Y - point2.Y;
            return _control.TopLevelControl.ClientSize.Height - _control.Bottom - num;
        }

        public bool IsIdle
        {
            get { _lastIsIdle = !_lastIsIdle; return _lastIsIdle; }
        }

        public IWindowInfo WindowInfo { get; }
    }
}

