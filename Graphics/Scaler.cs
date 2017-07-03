using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Render
{
    public class Scaler : IScaler
    {
        void IScaler.Scale(double x, double y, double z)
        {
            GL.Scale((float)x, (float)y, (float)z);
        }
    }
}
