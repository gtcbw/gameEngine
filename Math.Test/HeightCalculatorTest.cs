using Math.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Math.Test
{
    [TestClass]
    public class HeightCalculatorTest
    {
        int sideLength = 3;
        int meters = 1;
        float[] heightValues = new float[9];

        public HeightCalculatorTest()
        {
            heightValues[0] = 0;
            heightValues[1] = 1;
            heightValues[2] = 0;
            heightValues[3] = 1;
            heightValues[4] = 2;
            heightValues[5] = 0;
            heightValues[6] = 0;
            heightValues[7] = 0;
            heightValues[8] = 0;
        }

        [TestMethod]
        public void OutsideIsDefaultValue()
        {
            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, sideLength, meters);

            Assert.AreEqual(0.0, heightCalculator.CalculateHeight(-1, 0));
            Assert.AreEqual(0.0, heightCalculator.CalculateHeight(-1, -1));
            Assert.AreEqual(0.0, heightCalculator.CalculateHeight(3, 0));
            Assert.AreEqual(0.0, heightCalculator.CalculateHeight(3, 3));
        }

        [TestMethod]
        public void CornerValues()
        {
            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, sideLength, meters);

            Assert.AreEqual(0.0, heightCalculator.CalculateHeight(0, 0));
            Assert.AreEqual(1.0, heightCalculator.CalculateHeight(1, 0));
            Assert.AreEqual(1.0, heightCalculator.CalculateHeight(0, 1));
            Assert.AreEqual(2.0, heightCalculator.CalculateHeight(1, 1));
        }

        [TestMethod]
        public void InterpolationInTheMiddle()
        {
            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, sideLength, meters);

            Assert.AreEqual(1.0, heightCalculator.CalculateHeight(0.5, 0.5));
            Assert.AreEqual(1.0, heightCalculator.CalculateHeight(0.75, 0.25));
            Assert.AreEqual(1.0, heightCalculator.CalculateHeight(0.25, 0.75));
        }

        [TestMethod]
        public void InterpolationOnTheEdge()
        {
            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, sideLength, meters);

            Assert.AreEqual(0.5, heightCalculator.CalculateHeight(0.5, 0.0));
            Assert.AreEqual(0.5, heightCalculator.CalculateHeight(0.0, 0.5));
            Assert.AreEqual(1.5, heightCalculator.CalculateHeight(1.0, 0.5));
            Assert.AreEqual(1.5, heightCalculator.CalculateHeight(0.5, 1.0));
        }

        [TestMethod]
        public void InterpolationSomewhereBetween()
        {
            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, sideLength, meters);

            Assert.AreEqual(0.5, heightCalculator.CalculateHeight(0.25, 0.25));
            Assert.AreEqual(1.5, heightCalculator.CalculateHeight(0.75, 0.75));
            Assert.IsTrue(heightCalculator.CalculateHeight(0.95, 0.95) < 2.0);
            Assert.IsTrue(heightCalculator.CalculateHeight(0.95, 0.95) > 1.8);
            Assert.IsTrue(heightCalculator.CalculateHeight(0.05, 0.05) > 0);
            Assert.IsTrue(heightCalculator.CalculateHeight(0.05, 0.05) < 0.15);
        }
    }
}
