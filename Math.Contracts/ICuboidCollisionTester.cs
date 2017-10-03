using World.Model;

namespace Math.Contracts
{
    public interface ICuboidCollisionTester
    {
        bool CuboidsCollide(Cuboid cuboidOne, Position positionOne, Cuboid cuboidTwo, Position positionTwo);

        bool CuboidOneWithoutCenterCollides(Cuboid cuboidOne, Position positionOne, Cuboid cuboidTwo);

        bool CuboidsWithoutCenterCollide(Cuboid cuboidOne, Position positionOne, Cuboid cuboidTwo);
    }
}
