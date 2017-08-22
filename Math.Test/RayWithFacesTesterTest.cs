using Math.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using World.Model;

namespace Math.Test
{
    [TestClass]
    public class RayWithFacesTesterTest
    {
        [TestMethod]
        public void CollisionIsFoundCorrectly()
        {
            IRayWithFacesTester rayWithFacesTester = new RayWithFacesTester(new IntersectionCalculatorWithoutFaceCulling(), new ObtuseAngleTester(), new PositionDistanceTester());

            Face[] faces = new Face[]
            {
                new Face
                {
                    Normal = new double[] { 0, 0, -1 },
                    Triangles = new Triangle[]
                    {
                        new Triangle
                        {
                            Corner1 = new double[] { 1, -1, -1 },
                            Corner2 = new double[] { 0, 2, -1 },
                            Corner3 = new double[] { -1, -1, -1 },
                        }
                    }
                },
                new Face
                {
                    Normal = new double[] { 0, 0, 1 },
                    Triangles = new Triangle[]
                    {
                        new Triangle
                        {
                            Corner1 = new double[] { 1, -1, 2 },
                            Corner2 = new double[] { 0, 2, 2 },
                            Corner3 = new double[] { -1, -1, 2 },
                        }
                    }
                },
                new Face
                {
                    Normal = new double[] { 0, 0, -1 },
                    Triangles = new Triangle[]
                    {
                        new Triangle
                        {
                            Corner1 = new double[] { 1, -1, 1 },
                            Corner2 = new double[] { 0, 2, 1 },
                            Corner3 = new double[] { -1, -1, 1 },
                        }
                    }
                }
            };

            Position collision = rayWithFacesTester.SearchCollision(new double[] { 0, 0, 0 }, new double[] { 0, 0, 1 }, faces);

            Assert.IsNotNull(collision);

            Assert.IsTrue(collision.X < 0.0001 && collision.X > -0.0001);
            Assert.IsTrue(collision.Y < 0.0001 && collision.Y > -0.0001);
            Assert.IsTrue(collision.Z < 1.0001 && collision.Z > 0.9999);
        }
    }
}
