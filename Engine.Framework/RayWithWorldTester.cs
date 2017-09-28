using Engine.Contracts.Models;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Engine.Framework
{
    public sealed class RayWithWorldTester
    {
        private readonly IRayWithMapTester _rayWithMapTester;
        private readonly IRayWithModelsTester _rayWithModelsTester;
        private readonly IComplexShapeProvider _complexShapeProvider;
        private readonly IComplexShapeProvider _vehicleCollisionModelProvider;

        public RayWithWorldTester(IRayWithMapTester rayWithMapTester,
            IRayWithModelsTester rayWithModelsTester,
            IComplexShapeProvider complexShapeProvider,
            IComplexShapeProvider vehicleCollisionModelProvider)
        {
            _rayWithMapTester = rayWithMapTester;
            _rayWithModelsTester = rayWithModelsTester;
            _complexShapeProvider = complexShapeProvider;
            _vehicleCollisionModelProvider = vehicleCollisionModelProvider;
        }

        public Position Test(Ray ray)
        {
            Position nearestPosition = _rayWithMapTester.FindCollisionWithMap(ray);
            double maxDistance = 120;

            if (nearestPosition != null)
            {
                maxDistance = System.Math.Sqrt((ray.StartPosition.X - nearestPosition.X) * (ray.StartPosition.X - nearestPosition.X)
                     + (ray.StartPosition.Y - nearestPosition.Y) * (ray.StartPosition.Y - nearestPosition.Y)
                    + (ray.StartPosition.Z - nearestPosition.Z) * (ray.StartPosition.Z - nearestPosition.Z));
            }

            IEnumerable<ComplexShapeInstance> models = _complexShapeProvider.GetComplexShapes();
            Position modelCollisionPosition = _rayWithModelsTester.TestRayWithModels(models, ray, maxDistance);

            if (modelCollisionPosition != null)
            {
                double newmaxDistance = System.Math.Sqrt((ray.StartPosition.X - modelCollisionPosition.X) * (ray.StartPosition.X - modelCollisionPosition.X)
                    + (ray.StartPosition.Y - modelCollisionPosition.Y) * (ray.StartPosition.Y - modelCollisionPosition.Y)
                    + (ray.StartPosition.Z - modelCollisionPosition.Z) * (ray.StartPosition.Z - modelCollisionPosition.Z));

                if (newmaxDistance < maxDistance)
                {
                    nearestPosition = modelCollisionPosition;
                    maxDistance = newmaxDistance;
                }
            }



            models = _vehicleCollisionModelProvider.GetComplexShapes();
            modelCollisionPosition = _rayWithModelsTester.TestRayWithModels(models, ray, maxDistance);

            if (modelCollisionPosition != null)
            {
                double newmaxDistance = System.Math.Sqrt((ray.StartPosition.X - modelCollisionPosition.X) * (ray.StartPosition.X - modelCollisionPosition.X)
                    + (ray.StartPosition.Y - modelCollisionPosition.Y) * (ray.StartPosition.Y - modelCollisionPosition.Y)
                    + (ray.StartPosition.Z - modelCollisionPosition.Z) * (ray.StartPosition.Z - modelCollisionPosition.Z));

                if (newmaxDistance < maxDistance)
                {
                    nearestPosition = modelCollisionPosition;
                    maxDistance = newmaxDistance;
                }
            }


            return nearestPosition;
        }
    }
}
