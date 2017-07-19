using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Math.Test
{
    [TestClass]
    public class ActiveFieldCalculatorTest
    {
        [TestMethod]
        public void AllFieldsAreFound()
        {
            ActiveFieldCalculator activeFieldCalculator = new ActiveFieldCalculator(10, 3);
            IEnumerable<int> result = activeFieldCalculator.CalculateActiveFields(new Position { X = 14, Y = 0, Z = 17 });

            Assert.AreEqual(9, result.Count());
            Assert.IsTrue(result.Contains(0));
            Assert.IsTrue(result.Contains(1));
            Assert.IsTrue(result.Contains(2));
            Assert.IsTrue(result.Contains(3));
            Assert.IsTrue(result.Contains(4));
            Assert.IsTrue(result.Contains(5));
            Assert.IsTrue(result.Contains(6));
            Assert.IsTrue(result.Contains(7));
            Assert.IsTrue(result.Contains(8));
        }

        [TestMethod]
        public void FourExistingFieldsAreFound()
        {
            ActiveFieldCalculator activeFieldCalculator = new ActiveFieldCalculator(10, 3);
            IEnumerable<int> result = activeFieldCalculator.CalculateActiveFields(new Position { X = 6, Y = 0, Z = 21 });

            Assert.AreEqual(4, result.Count());
            Assert.IsTrue(result.Contains(3));
            Assert.IsTrue(result.Contains(4));
            Assert.IsTrue(result.Contains(6));
            Assert.IsTrue(result.Contains(7));
        }

        [TestMethod]
        public void SixExistingFieldsAreFound()
        {
            ActiveFieldCalculator activeFieldCalculator = new ActiveFieldCalculator(10, 3);
            IEnumerable<int> result = activeFieldCalculator.CalculateActiveFields(new Position { X = 29, Y = 0, Z = 13 });

            Assert.AreEqual(6, result.Count());
            Assert.IsTrue(result.Contains(1));
            Assert.IsTrue(result.Contains(2));
            Assert.IsTrue(result.Contains(4));
            Assert.IsTrue(result.Contains(5));
            Assert.IsTrue(result.Contains(7));
            Assert.IsTrue(result.Contains(8));
        }
    }
}
