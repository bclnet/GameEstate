using OpenTK.Graphics.OpenGL;
using OpenTK.WpfTest.Renderer;

namespace OpenTK.WpfTest.Scenes
{
    public class CubeDemoScene : GLViewerControl
    {
        public CubeDemoScene()
        {
            //GLLoad += OnLoad;
            GLPaint += OnPaint;
        }

        //void OnLoad(object sender, EventArgs e)
        //{
        //    GLPaint += OnPaint;
        //}

        void OnPaint(object sender, RenderEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 4.0f);
            GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 4.0f);
            GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 1.0f, 4.0f);

            GL.End();
        }
    }
}
