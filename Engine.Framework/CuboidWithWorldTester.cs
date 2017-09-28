using Engine.Contracts.Models;
using World.Model;

namespace Engine.Framework
{
    public sealed class CuboidWithWorldTester : ICuboidWithWorldTester
    {
        private readonly ICuboidWithModelsTester _cuboidWithModelsTester;
        private readonly IComplexShapeProvider _complexShapeProvider;
        private readonly IComplexShapeProvider _vehicleCollisionModelProvider;

        public CuboidWithWorldTester(ICuboidWithModelsTester cuboidWithModelsTester,
            IComplexShapeProvider complexShapeProvider,
             IComplexShapeProvider vehicleCollisionModelProvider)
        {
            _cuboidWithModelsTester = cuboidWithModelsTester;
            _complexShapeProvider = complexShapeProvider;
            _vehicleCollisionModelProvider = vehicleCollisionModelProvider;
        }

        bool ICuboidWithWorldTester.ElementCollidesWithWorld(Position position, double sideLength, double height)
        {
            Cuboid cuboid = new Cuboid
            {
                Center = new Position(),
                SideLengthX = sideLength,
                SideLengthZ = sideLength,
                SideLengthY = height
            };

            if (_cuboidWithModelsTester.CuboidCollides(_complexShapeProvider.GetComplexShapes(), cuboid, position))
                return true;

            if (_cuboidWithModelsTester.CuboidCollides(_vehicleCollisionModelProvider.GetComplexShapes(), cuboid, position))
                return true;

            return false;
        }
    }
}
