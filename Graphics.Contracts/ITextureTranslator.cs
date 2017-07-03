namespace Graphics.Contracts
{
    public interface ITextureTranslator
    {
        void TranslateTexture(double x, double y);

        void Store();

        void Reset();
    }
}
