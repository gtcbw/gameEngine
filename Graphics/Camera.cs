using System;
using Graphics.Contracts;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Engine.Contracts;
using World.Model;

namespace Graphics
{
    public sealed class Camera : ICamera
    {
        private double _aspectRatio;
        private IPlayerViewRayProvider _playerViewRayProvider;

        public Camera(double aspectRatio, IPlayerViewRayProvider playerViewRayProvider)
        {
            _aspectRatio = aspectRatio;
            _playerViewRayProvider = playerViewRayProvider;
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

        void ICamera.SetInGamePerspective()
        {
            GL.MatrixMode(MatrixMode.Projection);

            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 3, (float)_aspectRatio, 0.15f, 400.0f);
            GL.LoadMatrix(ref matrix);

            GL.MatrixMode(MatrixMode.Modelview);

            UpdatePosition();

            GL.Enable(EnableCap.DepthTest);
        }
        private void UpdatePosition()
        {
            Ray ray = _playerViewRayProvider.GetPlayerViewRay();

            Matrix4 modelview = Matrix4.LookAt(
            (float)ray.StartPosition.X,
                (float)ray.StartPosition.Y,
                (float)ray.StartPosition.Z,
                (float)(ray.StartPosition.X + ray.Direction.X),
                (float)(ray.StartPosition.Y + ray.Direction.Y),
                (float)(ray.StartPosition.Z + ray.Direction.Z),
                0, 1, 0
                );

            GL.LoadMatrix(ref modelview);
        }
    }
}
