using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkContracts;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using Render.Contracts;

namespace Render
{
    public class TextureLoader : ITextureLoader
    {
        private ITextureChanger _textureChanger;

        public TextureLoader(ITextureChanger textureChanger)
        {
            _textureChanger = textureChanger;
        }

        public ITexture LoadTexture(string texturePath, bool mipMap)
        {
            int id = GL.GenTexture();
            
            _textureChanger.SetTexture(id);

            bool hasAlphaChannel = texturePath.EndsWith(".png");

            Bitmap bitmap = new Bitmap(texturePath);
            BitmapData bitmapData;

            bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                hasAlphaChannel ? System.Drawing.Imaging.PixelFormat.Format32bppArgb : System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, hasAlphaChannel ? PixelInternalFormat.Rgba : PixelInternalFormat.Rgb, bitmapData.Width, bitmapData.Height, 0,
                hasAlphaChannel ? OpenTK.Graphics.OpenGL.PixelFormat.Bgra : OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, bitmapData.Scan0);

            bitmap.UnlockBits(bitmapData);

            if (mipMap)
            {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            }

            Texture texture = new Texture 
            { 
                TextureId = id,
                ResolutionX = bitmap.Width,
                ResolutionY = bitmap.Height,
                HasAlphaChannel = hasAlphaChannel
            };

            return texture;
        }

        public void DeleteTexture(ITexture texture)
        {
            GL.DeleteTexture(texture.TextureId);
        }
    }
}
