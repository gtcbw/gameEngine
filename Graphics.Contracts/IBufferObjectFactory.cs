namespace Graphics.Contracts
{
    public interface IBufferObjectFactory
    {
        uint GenerateIndexBuffer(ushort[] indexArray);
        uint GenerateVertexBuffer(float[] vertexArray);
        uint GenerateTextureCoordBuffer(float[] texCoordArray);
        void Delete(uint bufferId);
    }
}
