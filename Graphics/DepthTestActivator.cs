using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class DepthTestActivator : IDepthTestActivator
    {
        void IDepthTestActivator.Activate()
        {
            GL.Enable(EnableCap.DepthTest);
        }

        void IDepthTestActivator.Deactivate()
        {
            GL.Disable(EnableCap.DepthTest);
        }
    }
}
