using System;
using Graphics.Contracts;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class Camera : ICamera
    {
        private double _aspectRatio;

        public Camera(double aspectRatio)
        {
            _aspectRatio = aspectRatio;
        }

        void ICamera.SetDefaultPerspective()
        {
            GL.MatrixMode(MatrixMode.Projection);

            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 3, (float)_aspectRatio, 0.1f, 20.0f);
            GL.LoadMatrix(ref matrix);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Disable(EnableCap.DepthTest);
        }
    }
}
