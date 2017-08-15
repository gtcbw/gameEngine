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
    public class IntersectionCalculatorTest
    {
        [TestMethod]
        public void TestPositiveIntersectionWithCulling()
        {
            IIntersectionCalculator intersectionCalculator = new IntersectionCalculatorWithFaceCulling();

            Ray ray = new Ray { StartPosition = new Position { X = 0, Y = 0, Z = 0 }, Direction = new Vector { X = 0, Y = 1, Z = 1 } };
            Position corner1 = new Position { X = -1, Y = -0.5, Z = 3};
            Position corner2 = new Position { X = 0, Y = 5, Z = 4 };
            Position corner3 = new Position { X = 1, Y = -0.5, Z = 3 };
            

            double[] orig = new double[3] { ray.StartPosition.X, ray.StartPosition.Y, ray.StartPosition.Z };
            double[] dir = new double[3] { ray.Direction.X, ray.Direction.Y, ray.Direction.Z };
            double[] vert0 = new double[3] { corner1.X, corner1.Y, corner1.Z };
            double[] vert1 = new double[3] { corner2.X, corner2.Y, corner2.Z };
            double[] vert2 = new double[3] { corner3.X, corner3.Y, corner3.Z };

            Position hit = intersectionCalculator.RayHitsTriangle(orig, dir, vert0, vert1, vert2);

            Assert.IsNotNull(hit);
        }

        [TestMethod]
        public void TestNegativeIntersectionWithCulling()
        {
            IIntersectionCalculator intersectionCalculator = new IntersectionCalculatorWithFaceCulling();

            Ray ray = new Ray { StartPosition = new Position { X = 0, Y = 0, Z = 0 }, Direction = new Vector { X = 0, Y = -1, Z = 1 } };
            Position corner1 = new Position { X = -1, Y = -0.5, Z = 3 };
            Position corner2 = new Position { X = 0, Y = 5, Z = 4 };
            Position corner3 = new Position { X = 1, Y = -0.5, Z = 3 };

            double[] orig = new double[3] { ray.StartPosition.X, ray.StartPosition.Y, ray.StartPosition.Z };
            double[] dir = new double[3] { ray.Direction.X, ray.Direction.Y, ray.Direction.Z };
            double[] vert0 = new double[3] { corner1.X, corner1.Y, corner1.Z };
            double[] vert1 = new double[3] { corner2.X, corner2.Y, corner2.Z };
            double[] vert2 = new double[3] { corner3.X, corner3.Y, corner3.Z };

            Position hit = intersectionCalculator.RayHitsTriangle(orig, dir, vert0, vert1, vert2);

            Assert.IsNull(hit);
        }

        [TestMethod]
        public void TestPositiveIntersectionWithoutCulling()
        {
            IIntersectionCalculator intersectionCalculator = new IntersectionCalculatorWithoutFaceCulling();

            Ray ray = new Ray { StartPosition = new Position { X = 0, Y = 0, Z = 0 }, Direction = new Vector { X = 0, Y = 1, Z = 1 } };
            Position corner1 = new Position { X = -1, Y = -0.5, Z = 3 };
            Position corner2 = new Position { X = 0, Y = 5, Z = 4 };
            Position corner3 = new Position { X = 1, Y = -0.5, Z = 3 };


            double[] orig = new double[3] { ray.StartPosition.X, ray.StartPosition.Y, ray.StartPosition.Z };
            double[] dir = new double[3] { ray.Direction.X, ray.Direction.Y, ray.Direction.Z };
            double[] vert0 = new double[3] { corner1.X, corner1.Y, corner1.Z };
            double[] vert1 = new double[3] { corner2.X, corner2.Y, corner2.Z };
            double[] vert2 = new double[3] { corner3.X, corner3.Y, corner3.Z };

            Position hit = intersectionCalculator.RayHitsTriangle(orig, dir, vert0, vert1, vert2);

            Assert.IsNotNull(hit);
        }

        [TestMethod]
        public void TestNegativeIntersectionWithoutCulling()
        {
            IIntersectionCalculator intersectionCalculator = new IntersectionCalculatorWithoutFaceCulling();

            Ray ray = new Ray { StartPosition = new Position { X = 0, Y = 0, Z = 0 }, Direction = new Vector { X = 0, Y = -1, Z = 1 } };
            Position corner1 = new Position { X = -1, Y = -0.5, Z = 3 };
            Position corner2 = new Position { X = 0, Y = 5, Z = 4 };
            Position corner3 = new Position { X = 1, Y = -0.5, Z = 3 };

            double[] orig = new double[3] { ray.StartPosition.X, ray.StartPosition.Y, ray.StartPosition.Z };
            double[] dir = new double[3] { ray.Direction.X, ray.Direction.Y, ray.Direction.Z };
            double[] vert0 = new double[3] { corner1.X, corner1.Y, corner1.Z };
            double[] vert1 = new double[3] { corner2.X, corner2.Y, corner2.Z };
            double[] vert2 = new double[3] { corner3.X, corner3.Y, corner3.Z };

            Position hit = intersectionCalculator.RayHitsTriangle(orig, dir, vert0, vert1, vert2);

            Assert.IsNull(hit);
        }
    }
}
