using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class ColorSetter : IColorSetter
    {
        void IColorSetter.SetColor(float red, float green, float blue)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Color3(red, green, blue);
        }

        void IColorSetter.DisableColor()
        {
            GL.Color3(1.0, 1.0, 1.0);
            GL.Enable(EnableCap.Texture2D);
        }
    }
}
