using OpenTK.Graphics.OpenGL;
using Graphics.Contracts;

namespace Graphics
{
    public sealed class FrameBufferFactory : IFrameBufferFactory
    {
        public FrameBuffer GenerateFrameBuffer(int resolutionX, int resolutionY)
        {
            int frameBufferId = GL.GenFramebuffer();

            //nötig?
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferId);

            int textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, resolutionX, resolutionY, 0,
               PixelFormat.Bgr, PixelType.UnsignedByte, System.IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, textureId, 0);
            GL.DrawBuffers(1, new DrawBuffersEnum[] { DrawBuffersEnum.ColorAttachment0 });

            int depthId = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthId);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, resolutionX, resolutionY);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthId);

            //nötig?
            UnbindFrameBuffer();

            return new FrameBuffer { FrameBufferId = frameBufferId,TextureId = textureId };
        }

        public void SetFrameBuffer(int frameBufferId)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferId);
        }

        public void UnbindFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
