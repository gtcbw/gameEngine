using Graphics.Contracts;

namespace Graphics
{
    public class Texture : ITexture
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
