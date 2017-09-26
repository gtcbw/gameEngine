using Math.Contracts;
using World.Model;

namespace Math
{
    public sealed class PositionDistanceComparer : IPositionDistanceComparer
    {
        public bool PositionIsNearerThan(IReadOnlyPosition positionOne, IReadOnlyPosition positionTwo, double distance)
        {
            return !PositionIsLargerThan(positionOne, positionTwo, distance);
        }

        public bool PositionIsLargerThan(IReadOnlyPosition positionOne, IReadOnlyPosition positionTwo, double distance)
        {
            if (positionOne.X < positionTwo.X)
            {
                if (positionTwo.X - positionOne.X > distance)
                    return true;
            }
            else
            {
                if (positionOne.X - positionTwo.X > distance)
                    return true;
            }

            if (positionOne.Z < positionTwo.Z)
            {
                if (positionTwo.Z - positionOne.Z > distance)
                    return true;
            }
            else
            {
                if (positionOne.Z - positionTwo.Z > distance)
                    return true;
            }
            return false;
        }
    }
}
