using OpenTK.Graphics.OpenGL;
using Graphics.Contracts;

namespace Graphics
{
    public sealed class ViewPortSetter
    {
        public void SetViewport(int x, int y)
        {
            GL.Viewport(0, 0, x, y);
        }
    }
}
