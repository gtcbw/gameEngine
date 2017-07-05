using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class LightSwitch : ILightSwitch
    {
        void ILightSwitch.On()
        {
            GL.Enable(EnableCap.Lighting);
        }

        void ILightSwitch.Off()
        {
            GL.Disable(EnableCap.Lighting);
        }
    }
}
