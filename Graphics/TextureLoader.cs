using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using Graphics.Contracts;

namespace Graphics
{
    public sealed class TextureLoader : ITextureLoader
    {
        private ITextureChanger _textureChanger;
        private string _folder;

        public TextureLoader(ITextureChanger textureChanger, string folder)
        {
            _textureChanger = textureChanger;
            _folder = folder;
        }

        ITexture ITextureLoader.LoadTexture(string texturePath, bool mipmap, bool fullPath)
        {
            int id = GL.GenTexture();
            
            _textureChanger.SetTexture(id);

            bool hasAlphaChannel = texturePath.EndsWith(".png");

            Bitmap bitmap = fullPath ? new Bitmap(texturePath) :  new Bitmap($"{_folder}\\{texturePath}");
            BitmapData bitmapData;

            bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                hasAlphaChannel ? System.Drawing.Imaging.PixelFormat.Format32bppArgb : System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, hasAlphaChannel ? PixelInternalFormat.Rgba : PixelInternalFormat.Rgb, bitmapData.Width, bitmapData.Height, 0,
                hasAlphaChannel ? OpenTK.Graphics.OpenGL.PixelFormat.Bgra : OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, bitmapData.Scan0);

            bitmap.UnlockBits(bitmapData);

            if (mipmap)
            {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapNearest);
            }
            else
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            Texture texture = new Texture 
            { 
                TextureId = id,
                ResolutionX = bitmap.Width,
                ResolutionY = bitmap.Height,
                HasAlphaChannel = hasAlphaChannel
            };

            bitmap.Dispose();

            return texture;
        }

        public void DeleteTexture(ITexture texture)
        {
            GL.DeleteTexture(texture.TextureId);
        }
    }
}
