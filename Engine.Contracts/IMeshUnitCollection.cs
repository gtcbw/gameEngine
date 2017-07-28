using Graphics.Contracts;

namespace Engine.Contracts
{
    public interface IMeshUnitCollection
    {
        void AddMeshUnit(int id, BufferedMeshUnit unit);

        void RemoveMeshUnit(int id);
    }
}
