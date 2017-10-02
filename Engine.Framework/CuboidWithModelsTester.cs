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
        private readonly Position _emptyPosition = new Position();

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
                    rotatedPosition = position;
                
                if (_cuboidCollisionTester.CuboidsCollide(cuboid, rotatedPosition, instance.ComplexShape.MainCuboid, _emptyPosition))
                {
                    if (instance.ComplexShape.SubCuboids == null)
                        return true;

                    foreach (Cuboid subCuboid in instance.ComplexShape.SubCuboids)
                    {
                        if (_cuboidCollisionTester.CuboidsCollide(cuboid, rotatedPosition, subCuboid, subCuboid.Center))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
