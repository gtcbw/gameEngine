namespace Graphics.Contracts
{
    public interface ITexture
    {
        int TextureId { get; }

        bool HasAlphaChannel { get; }

        int ResolutionX { get; }

        int ResolutionY { get; }
    }
}
