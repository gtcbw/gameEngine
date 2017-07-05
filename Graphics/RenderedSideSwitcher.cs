using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class RenderedSideSwitcher : IRenderedSideSwitcher
    {
        void IRenderedSideSwitcher.SwitchToFrontSide()
        {
            GL.CullFace(CullFaceMode.Front);
        }

        void IRenderedSideSwitcher.SwitchToBackSide()
        {
            GL.CullFace(CullFaceMode.Back);
        }
    }
}
