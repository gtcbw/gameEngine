using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class Initializer
    {
        public static void Initialize()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.ClearDepth(1.0);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.02f);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Fog(FogParameter.FogStart, 50.0f);
            GL.Fog(FogParameter.FogEnd, 100.0f);
            GL.Fog(FogParameter.FogDensity, 0.5f);
            GL.Hint(HintTarget.FogHint, HintMode.Fastest);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
        }
    }
}
