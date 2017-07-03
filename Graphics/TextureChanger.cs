using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Render
{
    public class TextureChanger : ITextureChanger
    {
        private int _textureId;

        public TextureChanger()
        {
            _textureId = -1;
        }

        void ITextureChanger.SetTexture(int textureId)
        {
            if (_textureId == textureId)
                return;

            _textureId = textureId;
            GL.BindTexture(TextureTarget.Texture2D, textureId);
        }
    }
}
