using Graphics.Contracts;

namespace Engine.Contracts
{
    public interface IMeshUnitCreator
    {
        VertexBufferUnit CreateMeshUnit(float[] vertices);

        void DeleteMeshUnit(VertexBufferUnit unit);
    }
}
