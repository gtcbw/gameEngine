using Engine.Contracts.Models;
using World.Model;

namespace Engine.Framework
{
    public sealed class CuboidWithWorldTester : ICuboidWithWorldTester
    {
        private readonly ICuboidWithModelsTester _cuboidWithModelsTester;
        private readonly IComplexShapeProvider _complexShapeProvider;

        public CuboidWithWorldTester(ICuboidWithModelsTester cuboidWithModelsTester,
            IComplexShapeProvider complexShapeProvider)
        {
            _cuboidWithModelsTester = cuboidWithModelsTester;
            _complexShapeProvider = complexShapeProvider;
        }

        bool ICuboidWithWorldTester.ElementCollidesWithWorld(Position position, double sideLength, double height)
        {
            Cuboid cuboid = new Cuboid { Center = new Position(), SideLengthX = sideLength, SideLengthZ = sideLength, SideLengthY = height };
            return _cuboidWithModelsTester.CuboidCollides(_complexShapeProvider.GetComplexShapes(), cuboid, position);
        }
    }
}
