using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class MatrixManager : IMatrixManager
    {
        void IMatrixManager.Store()
        {
            GL.PushMatrix();
        }

        void IMatrixManager.Reset()
        {
            GL.PopMatrix();
        }

        void IMatrixManager.SetTextureMode()
        {
            GL.MatrixMode(MatrixMode.Texture);
        }

        void IMatrixManager.SetModelMode()
        {
            GL.MatrixMode(MatrixMode.Modelview);
        }
    }
}
