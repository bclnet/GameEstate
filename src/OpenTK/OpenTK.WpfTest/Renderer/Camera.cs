using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Numerics;
using NVector2 = System.Numerics.Vector2;
using NVector3 = System.Numerics.Vector3;

namespace OpenTK.WpfTest.Renderer
{
    public class Camera
    {
        const float CAMERASPEED = 300f; // Per second
        const float FOV = OpenTK.MathHelper.PiOver4;

        public NVector3 Location { get; private set; }
        public float Pitch { get; private set; }
        public float Yaw { get; private set; }
        public float Scale { get; private set; } = 1.0f;

        Matrix4x4 ProjectionMatrix;
        public Matrix4x4 CameraViewMatrix { get; private set; }
        public Matrix4x4 ViewProjectionMatrix { get; private set; }
        public Frustum ViewFrustum { get; } = new Frustum();

        // Set from outside this class by forms code
        public bool MouseOver { get; set; }

        NVector2 WindowSize;
        float AspectRatio;

        bool MouseDragging;

        NVector2 MouseDelta;
        NVector2 MousePreviousPosition;

        KeyboardState KeyboardState;

        public Camera()
        {
            Location = new NVector3(1);
            LookAt(new NVector3(0));
        }

        void RecalculateMatrices()
        {
            CameraViewMatrix = Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateLookAt(Location, Location + GetForwardVector(), NVector3.UnitZ);
            ViewProjectionMatrix = CameraViewMatrix * ProjectionMatrix;
            ViewFrustum.Update(ViewProjectionMatrix);
        }

        // Calculate forward vector from pitch and yaw
        NVector3 GetForwardVector() => new((float)(Math.Cos(Yaw) * Math.Cos(Pitch)), (float)(Math.Sin(Yaw) * Math.Cos(Pitch)), (float)Math.Sin(Pitch));

        NVector3 GetRightVector() => new((float)Math.Cos(Yaw - OpenTK.MathHelper.PiOver2), (float)Math.Sin(Yaw - OpenTK.MathHelper.PiOver2), 0);

        public void SetViewportSize(int viewportWidth, int viewportHeight)
        {
            // Store window size and aspect ratio
            AspectRatio = viewportWidth / (float)viewportHeight;
            WindowSize = new NVector2(viewportWidth, viewportHeight);

            // Calculate projection matrix
            ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(FOV, AspectRatio, 1.0f, 40000.0f);

            RecalculateMatrices();

            // setup viewport
            GL.Viewport(0, 0, viewportWidth, viewportHeight);

            // temp
            var projection = ToOpenTK(ProjectionMatrix);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        public OpenTK.Matrix4 ToOpenTK(Matrix4x4 m) => new OpenTK.Matrix4(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43, m.M44);

        public void CopyFrom(Camera fromOther)
        {
            AspectRatio = fromOther.AspectRatio;
            WindowSize = fromOther.WindowSize;
            Location = fromOther.Location;
            Pitch = fromOther.Pitch;
            Yaw = fromOther.Yaw;
            ProjectionMatrix = fromOther.ProjectionMatrix;
            CameraViewMatrix = fromOther.CameraViewMatrix;
            ViewProjectionMatrix = fromOther.ViewProjectionMatrix;
            ViewFrustum.Update(ViewProjectionMatrix);
        }

        public void SetLocation(NVector3 location)
        {
            Location = location;
            RecalculateMatrices();
        }

        public void SetLocationPitchYaw(NVector3 location, float pitch, float yaw)
        {
            Location = location;
            Pitch = pitch;
            Yaw = yaw;
            RecalculateMatrices();
        }

        public void LookAt(NVector3 target)
        {
            var dir = NVector3.Normalize(target - Location);
            Yaw = (float)Math.Atan2(dir.Y, dir.X);
            Pitch = (float)Math.Asin(dir.Z);

            ClampRotation();
            RecalculateMatrices();
        }

        public void SetFromTransformMatrix(Matrix4x4 matrix)
        {
            Location = matrix.Translation;

            // Extract view direction from view matrix and use it to calculate pitch and yaw
            var dir = new NVector3(matrix.M11, matrix.M12, matrix.M13);
            Yaw = (float)Math.Atan2(dir.Y, dir.X);
            Pitch = (float)Math.Asin(dir.Z);

            RecalculateMatrices();
        }

        public void SetScale(float scale)
        {
            Scale = scale;
            RecalculateMatrices();
        }

        public void Tick(float deltaTime)
        {
            if (!MouseOver) return;

            // Use the keyboard state to update position
            HandleKeyboardInput(deltaTime);

            // Full width of the screen is a 1 PI (180deg)
            Yaw -= (float)Math.PI * MouseDelta.X / WindowSize.X;
            Pitch -= ((float)Math.PI / AspectRatio) * MouseDelta.Y / WindowSize.Y;

            ClampRotation();

            RecalculateMatrices();
        }

        public void HandleInput(MouseState mouseState, KeyboardState keyboardState)
        {
            KeyboardState = keyboardState;

            if (MouseOver && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!MouseDragging) { MouseDragging = true; MousePreviousPosition = new NVector2(mouseState.X, mouseState.Y); }

                var mouseNewCoords = new NVector2(mouseState.X, mouseState.Y);

                MouseDelta.X = mouseNewCoords.X - MousePreviousPosition.X;
                MouseDelta.Y = mouseNewCoords.Y - MousePreviousPosition.Y;

                MousePreviousPosition = mouseNewCoords;
            }

            if (!MouseOver || mouseState.LeftButton == ButtonState.Released) { MouseDragging = false; MouseDelta = default; }
        }

        void HandleKeyboardInput(float deltaTime)
        {
            var speed = CAMERASPEED * deltaTime;

            // Double speed if shift is pressed
            if (KeyboardState.IsKeyDown(Key.ShiftLeft)) speed *= 2;
            else if (KeyboardState.IsKeyDown(Key.F)) speed *= 10;

            if (KeyboardState.IsKeyDown(Key.W)) Location += GetForwardVector() * speed;
            if (KeyboardState.IsKeyDown(Key.S)) Location -= GetForwardVector() * speed;
            if (KeyboardState.IsKeyDown(Key.D)) Location += GetRightVector() * speed;
            if (KeyboardState.IsKeyDown(Key.A)) Location -= GetRightVector() * speed;
            if (KeyboardState.IsKeyDown(Key.Z)) Location += new NVector3(0, 0, -speed);
            if (KeyboardState.IsKeyDown(Key.Q)) Location += new NVector3(0, 0, speed);
        }

        // Prevent camera from going upside-down
        void ClampRotation()
        {
            if (Pitch >= OpenTK.MathHelper.PiOver2) Pitch = OpenTK.MathHelper.PiOver2 - 0.001f;
            else if (Pitch <= -OpenTK.MathHelper.PiOver2) Pitch = -OpenTK.MathHelper.PiOver2 + 0.001f;
        }
    }
}
