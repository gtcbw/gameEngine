using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IWalkPositionCalculator
    {
        Position CalculateNextPosition(Position currentPosition, Vector2D viewVectorXZ);
    }
}
