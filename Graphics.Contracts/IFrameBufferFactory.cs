namespace Graphics.Contracts
{
    public interface IFrameBufferFactory
    {
        FrameBuffer GenerateFrameBuffer(int resolutionX, int resolutionY);
        void SetFrameBuffer(int frameBufferId);
        void UnbindFrameBuffer();
    }
}
