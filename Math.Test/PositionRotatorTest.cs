using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Math.Test
{
    [TestClass]
    public class PositionRotatorTest
    {
        [TestMethod]
        public void Rotation180Degree()
        {
            PositionRotator positionRotator = new PositionRotator();

            double[] result = positionRotator.Rotate(0, 0, 1, 180);

            Assert.IsTrue(result[0] > -0.000001);
            Assert.IsTrue(result[0] < 0.000001);

            Assert.IsTrue(result[1] > -0.000001);
            Assert.IsTrue(result[1] < 0.000001);

            Assert.IsTrue(result[2] < -0.99999);
            Assert.IsTrue(result[2] > -1.000001);
        }

        [TestMethod]
        public void Rotation90Degree()
        {
            PositionRotator positionRotator = new PositionRotator();

            double[] result = positionRotator.Rotate(0, 0, 1, 90);

            Assert.IsTrue(result[0] > 0.99999);
            Assert.IsTrue(result[0] < 1.000001);

            Assert.IsTrue(result[1] > -0.000001);
            Assert.IsTrue(result[1] < 0.000001);

            Assert.IsTrue(result[2] > -0.000001);
            Assert.IsTrue(result[2] < 0.000001);
        }

        [TestMethod]
        public void Rotation45Degree()
        {
            PositionRotator positionRotator = new PositionRotator();

            double[] result = positionRotator.Rotate(0, 0, 1, 45);

            Assert.IsTrue(result[0] > 0.705);
            Assert.IsTrue(result[0] < 0.709);

            Assert.IsTrue(result[1] > -0.000001);
            Assert.IsTrue(result[1] < 0.000001);

            Assert.IsTrue(result[2] > 0.705);
            Assert.IsTrue(result[2] < 0.709);
        }

        [TestMethod]
        public void RotationNegative180Degree()
        {
            PositionRotator positionRotator = new PositionRotator();

            double[] result = positionRotator.Rotate(0, 0, 1, -180);

            Assert.IsTrue(result[0] > -0.000001);
            Assert.IsTrue(result[0] < 0.000001);

            Assert.IsTrue(result[1] > -0.000001);
            Assert.IsTrue(result[1] < 0.000001);

            Assert.IsTrue(result[2] < -0.99999);
            Assert.IsTrue(result[2] > -1.000001);
        }

        [TestMethod]
        public void Rotation540Degree()
        {
            PositionRotator positionRotator = new PositionRotator();

            double[] result = positionRotator.Rotate(0, 0, 1, 540);

            Assert.IsTrue(result[0] > -0.000001);
            Assert.IsTrue(result[0] < 0.000001);

            Assert.IsTrue(result[1] > -0.000001);
            Assert.IsTrue(result[1] < 0.000001);

            Assert.IsTrue(result[2] < -0.99999);
            Assert.IsTrue(result[2] > -1.000001);
        }

        [TestMethod]
        public void RotationFrom45To90Degree()
        {
            PositionRotator positionRotator = new PositionRotator();

            double[] result = positionRotator.Rotate(0.70710678118, 0, 0.70710678118, 45);

            Assert.IsTrue(result[0] > 0.99999);
            Assert.IsTrue(result[0] < 1.000001);

            Assert.IsTrue(result[1] > -0.000001);
            Assert.IsTrue(result[1] < 0.000001);

            Assert.IsTrue(result[2] > -0.000001);
            Assert.IsTrue(result[2] < 0.000001);
        }
    }
}
