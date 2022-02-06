using OpenTK.Graphics;
using OpenTK.Platform;

namespace OpenTK
{
    internal interface IGLControl
    {
        IGraphicsContext CreateContext(int major, int minor, GraphicsContextFlags flags);

        bool IsIdle { get; }

        IWindowInfo WindowInfo { get; }
    }
}

