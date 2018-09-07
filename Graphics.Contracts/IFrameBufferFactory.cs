namespace Graphics.Contracts
{
    public interface IFrameBufferFactory
    {
        FrameBuffer GenerateFrameBuffer();
        void SetFrameBuffer(int frameBufferId);
        void UnbindFrameBuffer();
    }
}
