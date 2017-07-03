using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public class AlphaTester : IAlphaTester
    {
        void IAlphaTester.Begin()
        {
            GL.Enable(EnableCap.AlphaTest);
        }

        void IAlphaTester.End()
        {
            GL.Disable(EnableCap.AlphaTest);
        }
    }
}
