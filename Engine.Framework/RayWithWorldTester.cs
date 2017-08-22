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

        public RayWithWorldTester(IRayWithMapTester rayWithMapTester,
            IRayWithModelsTester rayWithModelsTester)
        {
            _rayWithMapTester = rayWithMapTester;
            _rayWithModelsTester = rayWithModelsTester;
        }

        public void Test(Ray ray)
        {
            var position =_rayWithMapTester.FindCollisionWithMap(ray);
            double maxDistance = 120;

            if (position != null)
            {
                maxDistance =System.Math.Sqrt((ray.StartPosition.X - position.X) * (ray.StartPosition.X - position.X)
                    + (ray.StartPosition.Z - position.Z) * (ray.StartPosition.Z - position.Z));
            }


        }
    }
}
