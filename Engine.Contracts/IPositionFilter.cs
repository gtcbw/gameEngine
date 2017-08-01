using World.Model;

namespace Engine.Contracts
{
    public interface IPositionFilter
    {
        bool IsValid(Position position);
    }
}
