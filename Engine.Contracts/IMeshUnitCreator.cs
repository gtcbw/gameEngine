using Graphics.Contracts;

namespace Engine.Contracts
{
    public interface IMeshUnitCreator
    {
        BufferedMeshUnit CreateMeshUnit(float[] vertices);

        void DeleteMeshUnit(BufferedMeshUnit unit);
    }
}
