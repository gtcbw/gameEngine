using Engine.Contracts.Models;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Engine.Framework
{
    public sealed class RayWithModelsTester : IRayWithModelsTester
    {
        private IRayWithFacesTester _rayWithFacesTester;
        private readonly IPositionDistanceTester _positionDistanceTester;
        private readonly IObtuseAngleTester _obtuseAngleTester;
        private readonly IPositionRotator _positionRotator;

        public RayWithModelsTester(IRayWithFacesTester rayWithFacesTester,
            IPositionDistanceTester positionDistanceTester,
            IObtuseAngleTester obtuseAngleTester,
            IPositionRotator positionRotator)
        {
            _rayWithFacesTester = rayWithFacesTester;
            _positionDistanceTester = positionDistanceTester;
            _obtuseAngleTester = obtuseAngleTester;
            _positionRotator = positionRotator;
        }

        Position IRayWithModelsTester.TestRayWithModels(IEnumerable<ComplexShapeInstance> models, Ray ray, double maxDistance)
        {
            Position collisionPosition = null;

            foreach (ComplexShapeInstance instance in models)
            {
                double squareDistance = (ray.StartPosition.X - instance.Position.X) * (ray.StartPosition.X - instance.Position.X) 
                    + (ray.StartPosition.Z - instance.Position.Z) * (ray.StartPosition.Z - instance.Position.Z);

                double maxSquareDistance = (maxDistance + instance.ComplexShape.RadiusXZ) * (maxDistance + instance.ComplexShape.RadiusXZ);
                if (squareDistance > maxSquareDistance)
                    continue;

                double[] rayDirection = new double[]
                {
                    ray.Direction.X,
                    ray.Direction.Y,
                    ray.Direction.Z
                };

                if (squareDistance > instance.ComplexShape.RadiusXZ * instance.ComplexShape.RadiusXZ)
                {
                   double[] rayToModelCenter = new double[]
                   {
                        instance.Position.X - ray.StartPosition.X,
                        instance.Position.Y - ray.StartPosition.Y,
                        instance.Position.Z - ray.StartPosition.Z
                   };

                    if (_obtuseAngleTester.AngleIsOver90Degree(rayDirection, rayToModelCenter))
                        continue;
                }

                double[] rayStartPosition = new double[] 
                {
                    ray.StartPosition.X - instance.Position.X,
                    ray.StartPosition.Y - instance.Position.Y,
                    ray.StartPosition.Z - instance.Position.Z
                };

                rayDirection = Rotate(rayDirection, -instance.RotationXZ);
                rayStartPosition = Rotate(rayStartPosition, -instance.RotationXZ);

                Position position = _rayWithFacesTester.SearchCollision(rayStartPosition, rayDirection, instance.ComplexShape.Faces);

                if (position != null)
                {
                    Rotate(position, instance.RotationXZ);

                    position.X += instance.Position.X;
                    position.Y += instance.Position.Y;
                    position.Z += instance.Position.Z;

                    if (collisionPosition == null)
                        collisionPosition = position;
                    else if (_positionDistanceTester.FirstPositionIsNearerToPoint(position, collisionPosition, rayStartPosition))
                        collisionPosition = position;
                }
            }
            return collisionPosition;
        }

        private double[] Rotate(double[] values, double rotationXZ)
        {
            if (rotationXZ == 0.0)
                return values;

            return _positionRotator.Rotate(values[0], values[1], values[2], rotationXZ);
        }

        private void Rotate(Position position, double rotationXZ)
        {
            if (rotationXZ == 0.0)
                return;

            double[] rotation = _positionRotator.Rotate(position.X, position.Y, position.Z, rotationXZ);
            position.X = rotation[0];
            position.Y = rotation[1];
            position.Z = rotation[2];
        }
    }
}
