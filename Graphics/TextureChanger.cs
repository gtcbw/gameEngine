using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class TextureChanger : ITextureChanger
    {
        private int _textureId;

        public TextureChanger()
        {
            _textureId = -1;
        }

        void ITextureChanger.SetTexture(int textureId, int channel)
        {
            if (_textureId == textureId)
                return;

            _textureId = textureId;
            switch(channel)
            {
                case 0:
                    GL.ActiveTexture(TextureUnit.Texture0);
                    break;
                case 1:
                    GL.ActiveTexture(TextureUnit.Texture1);
                    break;
                case 2:
                    GL.ActiveTexture(TextureUnit.Texture2);
                    break;
                case 3:
                    GL.ActiveTexture(TextureUnit.Texture3);
                    break;
            }
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
        }
    }
}
