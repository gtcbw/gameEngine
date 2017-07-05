using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class WorldRotator : IWorldRotator
    {
        void IWorldRotator.RotateX(double degree)
        {
            GL.Rotate(degree, 1, 0, 0);
        }

        void IWorldRotator.RotateY(double degree)
        {
            GL.Rotate(degree, 0, 1, 0);
        }

        void IWorldRotator.RotateZ(double degree)
        {
            GL.Rotate(degree, 0, 0, 1);
        }
    }
}
