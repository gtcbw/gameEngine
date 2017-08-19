using Math.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Math.Test
{
    [TestClass]
    public class RayWithMapTesterTest
    {
        [TestMethod]
        public void NoCollisionOnMap()
        {
            float[] values = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            IHeightCalculator heightCalculator = new HeightCalculator(values, 3, 1);
            IRayWithMapTester rayWithMapTester = new RayWithMapTester(heightCalculator, 10);

            Ray ray = new Ray { StartPosition = new Position { X = 1, Y = 2.5, Z = 0.1 }, Direction = new Vector { X = 0, Y = 0.707, Z = 0.707 } };
            var position = rayWithMapTester.FindCollisionWithMap(ray);

            Assert.IsNull(position);
        }

        [TestMethod]
        public void FindCollisionOnMap()
        {
            float[] values = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2};
            IHeightCalculator heightCalculator = new HeightCalculator(values, 3, 1);
            IRayWithMapTester rayWithMapTester = new RayWithMapTester(heightCalculator, 10);

            Ray ray = new Ray { StartPosition = new Position { X = 1, Y = 3, Z = 0.1 }, Direction = new Vector { X = 0, Y = -0.707, Z = 0.707 } };
            var position = rayWithMapTester.FindCollisionWithMap(ray);

            Assert.IsNotNull(position);
        }
    }
}
