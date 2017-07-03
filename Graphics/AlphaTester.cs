using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Render
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
