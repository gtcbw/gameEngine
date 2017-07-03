using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public class Initializer
    {
        public static void Initialize(Configuration configuration)
        {
            Window window = new Window(configuration.Resolution.X, configuration.Resolution.Y);

            window.Resize += (sender, e) =>
            {
                GL.Viewport(0, 0, configuration.Resolution.X, configuration.Resolution.Y);
            };

            GL.Enable(EnableCap.Texture2D);
            GL.ClearDepth(1.0);
            GL.DepthFunc(DepthFunction.Less);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.02f);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
        }
    }
}
