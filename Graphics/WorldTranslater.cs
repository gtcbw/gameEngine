using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public class WorldTranslator : IWorldTranslator
    {
        void IWorldTranslator.TranslateWorld(double x, double y, double z)
        {
            GL.Translate(x, y, z);
        }

        void IWorldTranslator.Store()
        {
            GL.PushMatrix();
        }

        void IWorldTranslator.Reset()
        {
            GL.PopMatrix();
        }
    }
}
