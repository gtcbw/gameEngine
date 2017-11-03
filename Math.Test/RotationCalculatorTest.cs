using Math.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Math.Test
{
    [TestClass]
    public class RotationCalculatorTest
    {
        [TestMethod]
        public void tan()
        {
            var tan10 = System.Math.Tan(10.0 /180.0 * System.Math.PI);
            var degree10 = System.Math.Atan(tan10) * 180.0 / System.Math.PI;

            var tan10minus = System.Math.Tan(-10.0 / 180.0 * System.Math.PI);
            var degree10minus = System.Math.Atan(tan10minus) * 180.0 / System.Math.PI;

            var tan80 = System.Math.Tan(80 / 180.0 * System.Math.PI);
            var degree80 = System.Math.Atan(tan80) * 180.0 / System.Math.PI;

            var tan100 = System.Math.Tan(100.0 / 180.0 * System.Math.PI);
            var degree100 = System.Math.Atan(tan100) * 180.0 / System.Math.PI;

            var tan170 = System.Math.Tan(170.0 / 180.0 * System.Math.PI);
            var degree170 = System.Math.Atan(tan170) * 180.0 / System.Math.PI;

            var tan200 = System.Math.Tan(200.0 / 180.0 * System.Math.PI);
            var degree200 = System.Math.Atan(tan200) * 180.0 / System.Math.PI;

            var tan350 = System.Math.Tan(350.0 / 180.0 * System.Math.PI);
            var degree350 = System.Math.Atan(tan350) * 180.0 / System.Math.PI;
        }

        [TestMethod]
        public void Find45DegreeXZ()
        {
            IRotationCalculator rotationCalculator = new RotationCalculator();
            Vector vector = new Vector { X = 1, Z = -1 };


            var rotation = rotationCalculator.CalculateRotation(vector);


           Assert.AreEqual(45.0, rotation.DegreeXZ);
        }

        [TestMethod]
        public void Find315DegreeXZ()
        {
            IRotationCalculator rotationCalculator = new RotationCalculator();
            Vector vector = new Vector { X = 1, Z = 1 };


            var rotation = rotationCalculator.CalculateRotation(vector);


            Assert.AreEqual(315.0, rotation.DegreeXZ);
        }
    }
}
