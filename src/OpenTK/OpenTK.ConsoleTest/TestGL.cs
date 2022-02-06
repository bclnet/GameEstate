using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace OpenTK.ConsoleTest
{
    class TestGL
    {
        class Window : GameWindow
        {
            public Window()
                 : base(800, 600, GraphicsMode.Default, "OpenTK Quick Start Sample") { }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);

                CheckOpenGL();
                GL.ClearColor(0.2f, 0.3f, 0.3f, 1f);
                GL.Enable(EnableCap.DepthTest);
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);

                GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

                var projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projection);
            }

            protected override void OnUpdateFrame(FrameEventArgs e)
            {
                base.OnUpdateFrame(e);

                var state = Keyboard.GetState();
                if (state.IsKeyDown(Key.Escape))
                    Exit();
            }

            protected override void OnRenderFrame(FrameEventArgs e)
            {
                base.OnRenderFrame(e);

                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                var modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);

                GL.Begin(PrimitiveType.Triangles);

                GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 4.0f);
                GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 4.0f);
                GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 1.0f, 4.0f);

                GL.End();

                SwapBuffers();
            }
        }

        static bool _hasCheckedOpenGL;
        static void CheckOpenGL()
        {
            if (_hasCheckedOpenGL)
                return;
            _hasCheckedOpenGL = true;

            Console.WriteLine($"OpenGL version: {GL.GetString(StringName.Version)}");
            Console.WriteLine($"OpenGL vendor: {GL.GetString(StringName.Vendor)}");
            Console.WriteLine($"GLSL version: {GL.GetString(StringName.ShadingLanguageVersion)}");

            var extensions = new HashSet<string>();
            var count = GL.GetInteger(GetPName.NumExtensions);
            for (var i = 0; i < count; i++)
            {
                var extension = GL.GetString(StringNameIndexed.Extensions, i);
                if (!extensions.Contains(extension))
                    extensions.Add(extension);
            }

            if (extensions.Contains("GL_EXT_texture_filter_anisotropic"))
            {
                var maxTextureMaxAnisotropy = GL.GetInteger((GetPName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt);
                Console.WriteLine($"MaxTextureMaxAnisotropyExt: {maxTextureMaxAnisotropy}");
            }
            else
                Console.Error.WriteLine("GL_EXT_texture_filter_anisotropic is not supported");
        }

        public void Test()
        {
            using (var window = new Window())
            {
                window.Run(30f);
            }
        }
    }
}
