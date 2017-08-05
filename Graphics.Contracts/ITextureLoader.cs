namespace Graphics.Contracts
{
    public interface ITextureLoader
    {
        ITexture LoadTexture(string texturePath, bool mipmap = false);

        void DeleteTexture(ITexture texture);
    }
}
