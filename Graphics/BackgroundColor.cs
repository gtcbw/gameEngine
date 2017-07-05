using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Drawing;
using Graphics.Contracts;

namespace Graphics
{
    public sealed class BackgroundColor : IBackgroundColor
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
