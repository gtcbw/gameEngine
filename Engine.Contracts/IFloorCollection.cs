using Graphics.Contracts;

namespace Engine.Contracts
{
    public interface IFloorCollection
    {
        void AddFloorPart(int id, BufferedMeshUnit floorPart);

        void RemoveFloorPart(int id);
    }
}
