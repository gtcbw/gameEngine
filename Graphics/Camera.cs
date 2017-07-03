using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Contracts;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Render
{
    public class Camera : ICamera
    {
        private FrameworkContracts.IPlayerCamera _playerCamera;
        private bool _depthTestEnabled;
        private IScreen _screen;

        public Camera(FrameworkContracts.IPlayerCamera playerCamera, IScreen screen)
        {
            _playerCamera = playerCamera;
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

            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 3, (float)_screen.AspectRatio, 0.2f, 600.0f);
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
            Matrix4 modelview = Matrix4.LookAt(
            (float)_playerCamera.CameraPosition.PositionX,
                (float)_playerCamera.CameraPosition.PositionY,
                (float)_playerCamera.CameraPosition.PositionZ,
                (float)_playerCamera.LookAtPosition.PositionX,
                (float)_playerCamera.LookAtPosition.PositionY,
                (float)_playerCamera.LookAtPosition.PositionZ,
                0, 1, 0
                );

            GL.LoadMatrix(ref modelview);
        }
    }
}
