using Engine.Contracts.Models;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Engine.Framework
{
    public sealed class CuboidWithModelsTester : ICuboidWithModelsTester
    {
        private readonly ICuboidCollisionTester _cuboidCollisionTester;

        public CuboidWithModelsTester(ICuboidCollisionTester cuboidCollisionTester)
        {
            _cuboidCollisionTester = cuboidCollisionTester;
        }

        public bool CuboidCollides(IEnumerable<ComplexShapeInstance> models, Cuboid cuboid, Position position)
        {
            foreach (ComplexShapeInstance instance in models)
            {
                if (_cuboidCollisionTester.CuboidsCollide(cuboid, position, instance.ComplexShape.MainCuboid, instance.Position))
                {
                    foreach (Cuboid subCuboid in instance.ComplexShape.SubCuboids)
                    {
                        if (_cuboidCollisionTester.CuboidsCollide(cuboid, position, subCuboid, instance.Position))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
