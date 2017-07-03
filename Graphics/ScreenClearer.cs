using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public class ScreenClearer
    {
        public void CleanScreen()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}
