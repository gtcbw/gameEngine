using Graphics.Contracts;

namespace Graphics
{
    public sealed class Texture : ITexture
    {
        public int TextureId
        {
            set;
            get;
        }

        public bool HasAlphaChannel
        {
            get;
            set;
        }

        public int ResolutionX
        {
            get;
            set;
        }

        public int ResolutionY
        {
            get;
            set;
        }
    }
}
