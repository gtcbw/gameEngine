using Graphics.Contracts;

namespace Engine.Contracts
{
    public interface IMeshUnitCollection
    {
        void AddMeshUnit(int id, VertexBufferUnit unit);

        void RemoveMeshUnit(int id);
    }
}
