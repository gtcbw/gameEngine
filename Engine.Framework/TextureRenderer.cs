using Engine.Contracts;
using Graphics.Contracts;

namespace Engine.Framework
{
    public sealed class TextureRenderer : IRenderingElement
    {
        private readonly IRenderingElement renderingElement;
        private readonly ITexture texture;
        private readonly ITextureChanger textureChanger;

        public TextureRenderer(IRenderingElement renderingElement, 
            ITexture texture, 
            ITextureChanger textureChanger)
        {
            this.renderingElement = renderingElement;
            this.texture = texture;
            this.textureChanger = textureChanger;
        }

        void IRenderingElement.Render()
        {
            textureChanger.SetTexture(texture.TextureId);
            renderingElement.Render();
        }
    }
}
