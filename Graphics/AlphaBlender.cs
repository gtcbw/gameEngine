using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Render
{
    public class AlphaBlender : IAlphaBlender
    {

        void IAlphaBlender.BeginBlending()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        void IAlphaBlender.EndBlending()
        {
            GL.Disable(EnableCap.Blend);
            GL.Color4(1.0, 1.0, 1.0, 1.0);
        }

        void IAlphaBlender.SetOpacity(double percent)
        {
            GL.Color4(1.0, 1.0, 1.0, percent);
        }
    }
}
