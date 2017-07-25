using OpenTK.Graphics.OpenGL;
using Graphics.Contracts;

namespace Graphics
{
    public sealed class Fog : IFog
    {
        void IFog.SetColor(float [] color)
        {
            GL.Fog(FogParameter.FogColor, color);
        }

        void IFog.StartFog()
        {
            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);      

            GL.Enable(EnableCap.Fog);
        }

        void IFog.StopFog()
        {
            GL.Disable(EnableCap.Fog);
        }
    }
}
