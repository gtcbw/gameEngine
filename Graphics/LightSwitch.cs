using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Render
{
    public class LightSwitch : ILightSwitch
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
