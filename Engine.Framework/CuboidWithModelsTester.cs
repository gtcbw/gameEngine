using Engine.Contracts.Models;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Engine.Framework
{
    public sealed class CuboidWithModelsTester : ICuboidWithModelsTester
    {
        private readonly ICuboidCollisionTester _cuboidCollisionTester;
        private readonly IPositionRotator _positionRotator;

        public CuboidWithModelsTester(ICuboidCollisionTester cuboidCollisionTester,
            IPositionRotator positionRotator)
        {
            _cuboidCollisionTester = cuboidCollisionTester;
            _positionRotator = positionRotator;
        }

        public bool CuboidCollides(IEnumerable<ComplexShapeInstance> models, Cuboid cuboid, Position position)
        {
            foreach (ComplexShapeInstance instance in models)
            {
                Position rotatedPosition;
                if (instance.RotationXZ != 0.0)
                {
                    double[] rotatedValues = _positionRotator.Rotate(position.X - instance.Position.X, position.Y - instance.Position.Y, position.Z - instance.Position.Z, -instance.RotationXZ);
                    rotatedPosition = new Position
                    {
                        X = rotatedValues[0],
                        Y = rotatedValues[1],
                        Z = rotatedValues[2]
                    };
                }
                else
                {
                    rotatedPosition = new Position
                    {
                        X = position.X - instance.Position.X,
                        Y = position.Y - instance.Position.Y,
                        Z = position.Z - instance.Position.Z
                    };
                }

                if (_cuboidCollisionTester.CuboidsWithoutCenterCollide(cuboid, rotatedPosition, instance.ComplexShape.MainCuboid))
                {
                    if (instance.ComplexShape.SubCuboids == null)
                        return true;

                    foreach (Cuboid subCuboid in instance.ComplexShape.SubCuboids)
                    {
                        if (_cuboidCollisionTester.CuboidOneWithoutCenterCollides(cuboid, rotatedPosition, subCuboid))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
