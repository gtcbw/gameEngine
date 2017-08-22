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

        public RayWithModelsTester(IRayWithFacesTester rayWithFacesTester,
            IPositionDistanceTester positionDistanceTester)
        {
            _rayWithFacesTester = rayWithFacesTester;
            _positionDistanceTester = positionDistanceTester;
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


                double[] rayStartPosition = new double[] 
                {
                    ray.StartPosition.X - instance.Position.X,
                    ray.StartPosition.Y - instance.Position.Y,
                    ray.StartPosition.Z - instance.Position.Z
                };
                double[] rayDirection = new double[] 
                {
                    ray.Direction.X,
                    ray.Direction.Y,
                    ray.Direction.Z
                };

                Position position = _rayWithFacesTester.SearchCollision(rayStartPosition, rayDirection, instance.ComplexShape.Faces);

                if (position != null)
                {
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
    }
}
