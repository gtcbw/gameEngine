using World.Model;

namespace Engine.Contracts
{
    public interface IVertexByFieldCreator
    {
        float[] CreateVertices(FieldCoordinates field);
    }
}
