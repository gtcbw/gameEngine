using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Render
{
    public class DepthTestActivator : IDepthTestActivator
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
