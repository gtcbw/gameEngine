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
    public class IntersectionCalculatorTest
    {
        [TestMethod]
        public void TestPositiveIntersection()
        {
            IntersectionCalculator intersectionCalculator = new IntersectionCalculator();

            Ray ray = new Ray { StartPosition = new Position { X = 0, Y = 0, Z = 0 }, Direction = new Vector { X = 0, Y = 1, Z = 1 } };
            Position corner1 = new Position { X = -1, Y = -0.5, Z = 3};
            Position corner2 = new Position { X = 1, Y = -0.5, Z = 3 };
            Position corner3 = new Position { X = 0, Y = 5, Z = 4 };

            Position hit = intersectionCalculator.RayHitsTriangle(ray, corner1, corner2, corner3);

            Assert.IsNotNull(hit);
        }

        [TestMethod]
        public void TestNegativeIntersection()
        {
            IntersectionCalculator intersectionCalculator = new IntersectionCalculator();

            Ray ray = new Ray { StartPosition = new Position { X = 0, Y = 0, Z = 0 }, Direction = new Vector { X = 0, Y = -1, Z = 1 } };
            Position corner1 = new Position { X = -1, Y = -0.5, Z = 3 };
            Position corner2 = new Position { X = 1, Y = -0.5, Z = 3 };
            Position corner3 = new Position { X = 0, Y = 5, Z = 4 };

            Position hit = intersectionCalculator.RayHitsTriangle(ray, corner1, corner3, corner2);

            Assert.IsNull(hit);
        }
    }
}
