using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkContracts;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Drawing;
using Render.Contracts;

namespace Render
{
    public class BackgroundColor : IBackgroundColor
    {
        private Color _backgroundColor;

        public BackgroundColor(Color backgroundColor)
        {
            _backgroundColor = backgroundColor;
        }

        void IBackgroundColor.SetColor()
        {
            GL.ClearColor(new Color4(_backgroundColor.R, _backgroundColor.G, _backgroundColor.B, _backgroundColor.A));
        }
    }
}
