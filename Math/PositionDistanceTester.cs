using Math.Contracts;
using World.Model;

namespace Math
{
    public sealed class PositionDistanceTester : IPositionDistanceTester
    {
        bool IPositionDistanceTester.FirstPositionIsNearerToPoint(Position first, Position second, double[] point)
        {
            double squareDistanceFirst = ((first.X - point[0]) * (first.X - point[0])) +
                                    ((first.Y - point[1]) * (first.Y - point[1])) +
                                    ((first.Z - point[2]) * (first.Z - point[2]));

            double squareDistanceSecond = ((second.X - point[0]) * (second.X - point[0])) +
                ((second.Y - point[1]) * (second.Y - point[1])) +
                ((second.Z - point[2]) * (second.Z - point[2]));

            return squareDistanceSecond > squareDistanceFirst;
        }
    }
}
