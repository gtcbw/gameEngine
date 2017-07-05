using System;
using Graphics.Contracts;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class Camera : ICamera
    {
        private bool _depthTestEnabled;
        private IScreen _screen;

        public Camera(IScreen screen)
        {
            _screen = screen;
        }

        void ICamera.SetDefaultPerspective()
        {
            GL.MatrixMode(MatrixMode.Projection);

            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 3, (float)_screen.AspectRatio, 0.1f, 20.0f);
            GL.LoadMatrix(ref matrix);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            if (_depthTestEnabled)
            {
                GL.Disable(EnableCap.DepthTest);
                _depthTestEnabled = false;
            }
        }

        void ICamera.SetInGamePerspective()
        {
            GL.MatrixMode(MatrixMode.Projection);

            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 3, (float)_screen.AspectRatio, 0.15f, 400.0f);
            GL.LoadMatrix(ref matrix);

            GL.MatrixMode(MatrixMode.Modelview);

            UpdatePosition();

            if (!_depthTestEnabled)
            {
                GL.Enable(EnableCap.DepthTest);
                _depthTestEnabled = true;
            }
        }
        private void UpdatePosition()
        {
            //Matrix4 modelview = Matrix4.LookAt(
            //(float)_playerCamera.CameraPosition.PositionX,
            //    (float)_playerCamera.CameraPosition.PositionY,
            //    (float)_playerCamera.CameraPosition.PositionZ,
            //    (float)_playerCamera.LookAtPosition.PositionX,
            //    (float)_playerCamera.LookAtPosition.PositionY,
            //    (float)_playerCamera.LookAtPosition.PositionZ,
            //    0, 1, 0
            //    );

            //GL.LoadMatrix(ref modelview);
        }
    }
}
