namespace Graphics.Contracts
{
    public interface IAlphaBlender
    {
        void BeginBlending();

        void EndBlending();

        void SetOpacity(double percent);
    }
}
