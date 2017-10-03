using Math.Contracts;
using World.Model;

namespace Math
{
    public sealed class CuboidCollisionTester : ICuboidCollisionTester
    {
        bool ICuboidCollisionTester.CuboidsCollide(Cuboid cuboidOne, Position positionOne, Cuboid cuboidTwo, Position positionTwo)
        {
            if (System.Math.Abs(cuboidOne.Center.X + positionOne.X - (cuboidTwo.Center.X + positionTwo.X)) > cuboidOne.HalfSideLengthX + cuboidTwo.HalfSideLengthX)
                return false;

            if (System.Math.Abs(cuboidOne.Center.Z + positionOne.Z - (cuboidTwo.Center.Z + positionTwo.Z)) > cuboidOne.HalfSideLengthZ + cuboidTwo.HalfSideLengthZ)
                return false;

            if (System.Math.Abs(cuboidOne.Center.Y + positionOne.Y + cuboidOne.HalfSideLengthY - (cuboidTwo.Center.Y + positionTwo.Y + cuboidTwo.HalfSideLengthY)) > cuboidOne.HalfSideLengthY + cuboidTwo.HalfSideLengthY)
                return false;

            return true;
        }

        bool ICuboidCollisionTester.CuboidOneWithoutCenterCollides(Cuboid cuboidOne, Position positionOne, Cuboid cuboidTwo)
        {
            if (System.Math.Abs(positionOne.X - cuboidTwo.Center.X) > cuboidOne.HalfSideLengthX + cuboidTwo.HalfSideLengthX)
                return false;

            if (System.Math.Abs(positionOne.Z - cuboidTwo.Center.Z) > cuboidOne.HalfSideLengthZ + cuboidTwo.HalfSideLengthZ)
                return false;

            if (System.Math.Abs( positionOne.Y + cuboidOne.HalfSideLengthY - (cuboidTwo.Center.Y + cuboidTwo.HalfSideLengthY)) > cuboidOne.HalfSideLengthY + cuboidTwo.HalfSideLengthY)
                return false;

            return true;
        }

        bool ICuboidCollisionTester.CuboidsWithoutCenterCollide(Cuboid cuboidOne, Position positionOne, Cuboid cuboidTwo)
        {
            if (System.Math.Abs(positionOne.X) > cuboidOne.HalfSideLengthX + cuboidTwo.HalfSideLengthX)
                return false;

            if (System.Math.Abs(positionOne.Z) > cuboidOne.HalfSideLengthZ + cuboidTwo.HalfSideLengthZ)
                return false;

            if (System.Math.Abs(positionOne.Y + cuboidOne.HalfSideLengthY - cuboidTwo.HalfSideLengthY) > cuboidOne.HalfSideLengthY + cuboidTwo.HalfSideLengthY)
                return false;

            return true;
        }
    }
}
