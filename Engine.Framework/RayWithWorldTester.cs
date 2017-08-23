using Engine.Contracts.Models;
using Math.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Engine.Framework
{
    public sealed class RayWithWorldTester
    {
        private readonly IRayWithMapTester _rayWithMapTester;
        private readonly IRayWithModelsTester _rayWithModelsTester;
        private readonly IComplexShapeProvider _complexShapeProvider;

        public RayWithWorldTester(IRayWithMapTester rayWithMapTester,
            IRayWithModelsTester rayWithModelsTester,
            IComplexShapeProvider complexShapeProvider)
        {
            _rayWithMapTester = rayWithMapTester;
            _rayWithModelsTester = rayWithModelsTester;
            _complexShapeProvider = complexShapeProvider;
        }

        public Position Test(Ray ray)
        {
            var position =_rayWithMapTester.FindCollisionWithMap(ray);
            double maxDistance = 120;

            if (position != null)
            {
                maxDistance = System.Math.Sqrt((ray.StartPosition.X - position.X) * (ray.StartPosition.X - position.X)
                    + (ray.StartPosition.Z - position.Z) * (ray.StartPosition.Z - position.Z));
            }

            IEnumerable<ComplexShapeInstance> models = _complexShapeProvider.GetComplexShapes();
            Position modelCollisionPosition = _rayWithModelsTester.TestRayWithModels(models, ray, maxDistance);

            if (modelCollisionPosition != null)
            {
                double newmaxDistance = System.Math.Sqrt((ray.StartPosition.X - modelCollisionPosition.X) * (ray.StartPosition.X - modelCollisionPosition.X)
                    + (ray.StartPosition.Z - modelCollisionPosition.Z) * (ray.StartPosition.Z - modelCollisionPosition.Z));

                if (newmaxDistance < maxDistance)
                    return modelCollisionPosition;
            }

            return position;
        }
    }
}
