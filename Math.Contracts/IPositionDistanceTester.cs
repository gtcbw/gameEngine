
using World.Model;

namespace Math.Contracts
{
    public interface IPositionDistanceTester
    {
        bool FirstPositionIsNearerToPoint(Position first, Position second, double[] point);
    }
}
