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
    public class VectorHelperTest
    {
        [TestMethod]
        public void VectorHasUnitLength()
        {
            IVectorHelper vectorHelper = new VectorHelper();

            Vector vector = vectorHelper.CreateFromDegrees(45, 45);

            double length = System.Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z));

            Assert.IsTrue(length < 1.0001);
            Assert.IsTrue(length > 0.9999);

            vector = vectorHelper.CreateFromDegrees(30, 70);
            length = System.Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z));

            Assert.IsTrue(length < 1.0001);
            Assert.IsTrue(length > 0.9999);

            vector = vectorHelper.CreateFromDegrees(20, 50);
            length = System.Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z));

            Assert.IsTrue(length < 1.0001);
            Assert.IsTrue(length > 0.9999);
        }
    }
}
