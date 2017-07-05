namespace Graphics.Contracts
{
    public interface ITextureLoader
    {
        ITexture LoadTexture(string texturePath);

        void DeleteTexture(ITexture texture);
    }
}
