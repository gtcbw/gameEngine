namespace Graphics.Contracts
{
    public interface IBufferedMeshUnitFactory
    {
        BufferedMeshUnit GenerateBuffer(float[] vertexArray, ushort[] indexArray);
        void Delete(BufferedMeshUnit meshUnit);
    }
}
