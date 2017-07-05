using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class Scaler : IScaler
    {
        void IScaler.Scale(double x, double y, double z)
        {
            GL.Scale((float)x, (float)y, (float)z);
        }
    }
}
