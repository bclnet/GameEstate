using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS
{
    internal class NS
    {
        const string Library = "libdl.dylib";

        [DllImport("libdl.dylib")]
        static extern int dlclose(IntPtr handle);
        [DllImport("libdl.dylib")]
        static extern IntPtr dlopen(string fileName, int flags);
        [DllImport("libdl.dylib")]
        static extern IntPtr dlsym(IntPtr handle, string symbol);

        public static void FreeLibrary(IntPtr handle) => dlclose(handle);

        public static IntPtr GetAddress(IntPtr function)
        {
            var zero = IntPtr.Zero;
            if (NSIsSymbolNameDefined(function))
            {
                zero = NSLookupAndBindSymbol(function);
                if (zero != IntPtr.Zero)
                    zero = NSAddressOfSymbol(zero);
            }
            return zero;
        }

        public static IntPtr GetAddress(string function)
        {
            IntPtr address;
            IntPtr ptr = Marshal.AllocHGlobal((int)(function.Length + 2));
            try
            {
                Marshal.WriteByte(ptr, 0x5f);
                var num = 0;
                while (true)
                {
                    if (num >= function.Length)
                    {
                        Marshal.WriteByte(ptr, function.Length + 1, 0);
                        address = GetAddress(ptr);
                        break;
                    }
                    Marshal.WriteByte(ptr, num + 1, (byte)function[num]);
                    num++;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return address;
        }

        public static IntPtr GetSymbol(IntPtr handle, string symbol) => dlsym(handle, symbol);

        public static IntPtr LoadLibrary(string fileName) => dlopen(fileName, 2);

        [DllImport("libdl.dylib")]
        static extern IntPtr NSAddressOfSymbol(IntPtr symbol);
        [DllImport("libdl.dylib")]
        static extern bool NSIsSymbolNameDefined(IntPtr s);
        [DllImport("libdl.dylib")]
        static extern bool NSIsSymbolNameDefined(string s);
        [DllImport("libdl.dylib")]
        static extern IntPtr NSLookupAndBindSymbol(IntPtr s);
        [DllImport("libdl.dylib")]
        static extern IntPtr NSLookupAndBindSymbol(string s);
    }
}

