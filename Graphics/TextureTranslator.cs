using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class TextureTranslator : ITextureTranslator
    {

        void ITextureTranslator.TranslateTexture(double x, double y)
        {
            GL.Translate(x, y, 0);
        }

        void ITextureTranslator.Store()
        {
            GL.MatrixMode(MatrixMode.Texture);
            GL.PushMatrix();
        }

        void ITextureTranslator.Reset()
        {
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
        }
    }
}
