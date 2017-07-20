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
            List<FieldCoordinates> result = activeFieldCalculator.CalculateActiveFields(new Position { X = 14, Y = 0, Z = 17 }).ToList();

            Assert.AreEqual(9, result.Count());
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 0 && x.Z == 0 && x.ID == 0));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 1 && x.Z == 0 && x.ID == 1));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 2 && x.Z == 0 && x.ID == 2));

            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 0 && x.Z == 1 && x.ID == 3));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 1 && x.Z == 1 && x.ID == 4));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 2 && x.Z == 1 && x.ID == 5));

            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 0 && x.Z == 2 && x.ID == 6));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 1 && x.Z == 2 && x.ID == 7));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 2 && x.Z == 2 && x.ID == 8));
        }

        [TestMethod]
        public void FourExistingFieldsAreFound()
        {
            ActiveFieldCalculator activeFieldCalculator = new ActiveFieldCalculator(10, 3);
            IEnumerable<FieldCoordinates> result = activeFieldCalculator.CalculateActiveFields(new Position { X = 6, Y = 0, Z = 21 });

            Assert.AreEqual(4, result.Count());

            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 0 && x.Z == 1));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 1 && x.Z == 1));

            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 0 && x.Z == 2));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 1 && x.Z == 2));
        }

        [TestMethod]
        public void SixExistingFieldsAreFound()
        {
            ActiveFieldCalculator activeFieldCalculator = new ActiveFieldCalculator(10, 3);
            IEnumerable<FieldCoordinates> result = activeFieldCalculator.CalculateActiveFields(new Position { X = 29, Y = 0, Z = 13 });

            Assert.AreEqual(6, result.Count());

            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 1 && x.Z == 0));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 2 && x.Z == 0));

            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 1 && x.Z == 1));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 2 && x.Z == 1));

            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 1 && x.Z == 2));
            Assert.IsNotNull(result.FirstOrDefault(x => x.X == 2 && x.Z == 2));
        }
    }
}
