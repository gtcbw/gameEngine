using Math.Contracts;
using World.Model;

namespace Math
{
    public sealed class CuboidCollisionTester : ICuboidCollisionTester
    {
        bool ICuboidCollisionTester.CuboidsCollide(Cuboid cuboidOne, Position positionOne, Cuboid cuboidTwo, Position positionTwo)
        {
            if (System.Math.Abs(cuboidOne.Center.X + positionOne.X - (cuboidTwo.Center.X + positionTwo.X)) > (cuboidOne.SideLengthX / 2.0) + (cuboidTwo.SideLengthX / 2.0))
                return false;

            if (System.Math.Abs(cuboidOne.Center.Z + positionOne.Z - (cuboidTwo.Center.Z + positionTwo.Z)) > (cuboidOne.SideLengthZ / 2.0) + (cuboidTwo.SideLengthZ / 2.0))
                return false;

            if (System.Math.Abs(cuboidOne.Center.Y + positionOne.Y + cuboidOne.SideLengthY / 2.0 - (cuboidTwo.Center.Y + positionTwo.Y + cuboidTwo.SideLengthY / 2.0)) > (cuboidOne.SideLengthY / 2.0) + (cuboidTwo.SideLengthY / 2.0))
                return false;

            return true;
        }

        bool ICuboidCollisionTester.CuboidOneWithoutCenterCollides(Cuboid cuboidOne, Position positionOne, Cuboid cuboidTwo)
        {
            if (System.Math.Abs(positionOne.X - cuboidTwo.Center.X) > (cuboidOne.SideLengthX / 2.0) + (cuboidTwo.SideLengthX / 2.0))
                return false;

            if (System.Math.Abs(positionOne.Z - cuboidTwo.Center.Z) > (cuboidOne.SideLengthZ / 2.0) + (cuboidTwo.SideLengthZ / 2.0))
                return false;

            if (System.Math.Abs( positionOne.Y + cuboidOne.SideLengthY / 2.0 - (cuboidTwo.Center.Y + cuboidTwo.SideLengthY / 2.0)) > (cuboidOne.SideLengthY / 2.0) + (cuboidTwo.SideLengthY / 2.0))
                return false;

            return true;
        }

        bool ICuboidCollisionTester.CuboidsWithoutCenterCollide(Cuboid cuboidOne, Position positionOne, Cuboid cuboidTwo)
        {
            if (positionOne.X > (cuboidOne.SideLengthX / 2.0) + (cuboidTwo.SideLengthX / 2.0))
                return false;

            if (positionOne.Z > (cuboidOne.SideLengthZ / 2.0) + (cuboidTwo.SideLengthZ / 2.0))
                return false;

            if (System.Math.Abs(positionOne.Y + cuboidOne.SideLengthY / 2.0 - (cuboidTwo.SideLengthY / 2.0)) > (cuboidOne.SideLengthY / 2.0) + (cuboidTwo.SideLengthY / 2.0))
                return false;

            return true;
        }
    }
}
