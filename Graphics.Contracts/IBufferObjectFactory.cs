namespace Graphics.Contracts
{
    public interface IBufferObjectFactory
    {
        uint GenerateIndexBuffer(ushort[] indexArray);
        uint GenerateVertexBuffer(float[] vertexArray);
        uint GenerateTextureCoordBuffer(float[] texCoordArray);
        uint GenerateNormalBuffer(float[] normalArray);
        void Delete(uint bufferId);
    }
}
