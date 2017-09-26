using World.Model;

namespace Math.Contracts
{
    public interface IPositionDistanceComparer
    {
        bool PositionIsNearerThan(IReadOnlyPosition positionOne, IReadOnlyPosition positionTwo, double distance);

        bool PositionIsLargerThan(IReadOnlyPosition positionOne, IReadOnlyPosition positionTwo, double distance);
    }
}
