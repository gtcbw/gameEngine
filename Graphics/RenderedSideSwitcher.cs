using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Render
{
    public class RenderedSideSwitcher : IRenderedSideSwitcher
    {
        void IRenderedSideSwitcher.SwitchToFrontSide()
        {
            GL.CullFace(CullFaceMode.Front);
        }

        void IRenderedSideSwitcher.SwitchToBackSide()
        {
            GL.CullFace(CullFaceMode.Back);
        }
    }
}
