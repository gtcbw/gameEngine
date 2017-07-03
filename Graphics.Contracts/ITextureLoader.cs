namespace Graphics.Contracts
{
    public interface ITextureLoader
    {
        ITexture LoadTexture(string texturePath, bool mipMap);

        void DeleteTexture(ITexture texture);
    }
}
