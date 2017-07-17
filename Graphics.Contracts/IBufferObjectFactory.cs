namespace Graphics.Contracts
{
    public interface IBufferObjectFactory
    {
        uint GenerateIndexBuffer(ushort[] indexArray);
        uint GenerateVertexBuffer(float[] vertexArray);
        void Delete(uint bufferId);
    }
}
