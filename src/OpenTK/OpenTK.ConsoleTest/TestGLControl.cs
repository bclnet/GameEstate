using System;

namespace OpenTK.ConsoleTest
{
    class TestGLControl
    {
        GLControl _control = new GLControl();

        public void Test()
        {
            _control.BeginInit();
            _control.EndInit();
        }
    }
}
