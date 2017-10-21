using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class CullingController : ICullingController
    {
        void ICullingController.Off()
        {
            GL.Disable(EnableCap.CullFace);
        }

        void ICullingController.On()
        {
            GL.Enable(EnableCap.CullFace);
        }
    }
}
