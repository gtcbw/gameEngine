using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Render
{
    public class Initializer
    {
        public void Initialize()
        {
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
