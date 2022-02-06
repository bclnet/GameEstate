using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS.Carbon
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Rect
    {
        short top;
        short left;
        short bottom;
        short right;
        internal Rect(int left, int top, int width, int height) : this((short)left, (short)top, (short)width, (short)height) { }

        internal Rect(short _left, short _top, short _width, short _height)
        {
            top = _top;
            left = _left;
            bottom = (short)(_top + _height);
            right = (short)(_left + _width);
        }

        internal short X
        {
            get => left;
            set
            {
                var width = Width;
                left = value;
                right = (short)(left + width);
            }
        }
        internal short Y
        {
            get => top;
            set
            {
                var height = Height;
                top = value;
                bottom = (short)(top + height);
            }
        }
        internal short Width
        {
            get => (short)(right - left);
            set => right = (short)(left + value);
        }
        internal short Height
        {
            get => (short)(bottom - top);
            set => bottom = (short)(top + value);
        }
        public override string ToString() => $"Rect: [{X}, {Y}, {Width}, {Height}]";

        public Rectangle ToRectangle() => new Rectangle(X, Y, Width, Height);
    }
}

