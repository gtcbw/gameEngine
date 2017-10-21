using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class Translator : ITranslator
    {
        void ITranslator.Translate(double x, double y, double z)
        {
            GL.Translate(x, y, z);
        }
    }
}
