using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS.Carbon
{
    internal class API
    {
        const string carbon = "/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon";

        [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
        internal static extern OSStatus DMGetGDeviceByDisplayID(IntPtr displayID, out IntPtr displayDevice, bool failToMain);
        internal static Rect GetControlBounds(IntPtr control)
        {
            GetControlBounds(control, out var rect);
            return rect;
        }

        [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
        private static extern IntPtr GetControlBounds(IntPtr control, out Rect bounds);
        [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
        internal static extern IntPtr GetControlOwner(IntPtr control);
        [DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
        internal static extern IntPtr GetWindowPort(IntPtr windowRef);
    }
}

