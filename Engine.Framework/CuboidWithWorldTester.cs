using Engine.Contracts.Models;
using World.Model;

namespace Engine.Framework
{
    public sealed class CuboidWithWorldTester
    {
        private readonly ICuboidWithModelsTester _cuboidWithModelsTester;
        private readonly IComplexShapeProvider _complexShapeProvider;

        public CuboidWithWorldTester(ICuboidWithModelsTester cuboidWithModelsTester,
            IComplexShapeProvider complexShapeProvider)
        {
            _cuboidWithModelsTester = cuboidWithModelsTester;
            _complexShapeProvider = complexShapeProvider;
        }

        public bool CuboidCollidesWithWorld(Cuboid cuboid, Position position)
        {
            return _cuboidWithModelsTester.CuboidCollides(_complexShapeProvider.GetComplexShapes(), cuboid, position);
        }
    }
}
