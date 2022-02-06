using System;

namespace OpenTK.ConsoleTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new TestGLControl().Test();
            //new TestGL().Test();
        }
    }
}
