using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Math.Test
{
    [TestClass]
    public class ObtuseAngleTesterTest
    {
        [TestMethod]
        public void AngleIsObtuse()
        {
            ObtuseAngleTester obtuseAngleTester = new ObtuseAngleTester();

            double[] vector1 = new double[3] { 0, 1, 0};
            double[] vector2 = new double[3] { 0, -0.9, 0.1 };

            bool isObtuse = obtuseAngleTester.AngleIsObtuse(vector1, vector2);

            Assert.IsTrue(isObtuse);
        }

        [TestMethod]
        public void AngleIsNotObtuse()
        {
            ObtuseAngleTester obtuseAngleTester = new ObtuseAngleTester();

            double[] vector1 = new double[3] { 0, 1, 0 };
            double[] vector2 = new double[3] { 0, 0.9, 0.1 };

            bool isObtuse = obtuseAngleTester.AngleIsObtuse(vector1, vector2);

            Assert.IsFalse(isObtuse);
        }
    }
}
