using Math.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using World.Model;

namespace Math.Test
{
    [TestClass]
    public class CuboidCollisionTesterTest
    {
        [TestMethod]
        public void IdenticalCuboidsCollide()
        {
            Cuboid cuboidOne = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 1, SideLengthZ = 1 };
            Position positionOne = new Position();
            Cuboid cuboidTwo = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 1, SideLengthZ = 1 };
            Position positionTwo = new Position();

            ICuboidCollisionTester cuboidCollisionTester = new CuboidCollisionTester();

            Assert.IsTrue(cuboidCollisionTester.CuboidsCollide(cuboidOne, positionOne, cuboidTwo, positionTwo));
        }

        [TestMethod]
        public void CuboidsWithDifferentPositionCollide()
        {
            Cuboid cuboidOne = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 1, SideLengthZ = 1 };
            Position positionOne = new Position();
            Cuboid cuboidTwo = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 1, SideLengthZ = 1 };
            Position positionTwo = new Position { X = 0.8, Y = 0.9, Z = 0.4 };

            ICuboidCollisionTester cuboidCollisionTester = new CuboidCollisionTester();

            Assert.IsTrue(cuboidCollisionTester.CuboidsCollide(cuboidOne, positionOne, cuboidTwo, positionTwo));
        }

        [TestMethod]
        public void CuboidsWithDifferentPositionDoNotCollide()
        {
            Cuboid cuboidOne = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 1, SideLengthZ = 1 };
            Position positionOne = new Position();
            Cuboid cuboidTwo = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 1, SideLengthZ = 1 };
            Position positionTwo = new Position { X = 0.3, Y = 0.3, Z = 1.4 };

            ICuboidCollisionTester cuboidCollisionTester = new CuboidCollisionTester();

            Assert.IsFalse(cuboidCollisionTester.CuboidsCollide(cuboidOne, positionOne, cuboidTwo, positionTwo));
        }

        [TestMethod]
        public void CuboidsWithDifferentSideLengthCollide()
        {
            Cuboid cuboidOne = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 1, SideLengthZ = 1 };
            Position positionOne = new Position();
            Cuboid cuboidTwo = new Cuboid { Center = new Position(), SideLengthX = 11, SideLengthY = 0.6, SideLengthZ = 0.8 };
            Position positionTwo = new Position { X = 5.3, Y = 0.3, Z = 0.3 };

            ICuboidCollisionTester cuboidCollisionTester = new CuboidCollisionTester();

            Assert.IsTrue(cuboidCollisionTester.CuboidsCollide(cuboidOne, positionOne, cuboidTwo, positionTwo));
        }

        [TestMethod]
        public void CuboidsWithDifferentHeightCollide()
        {
            Cuboid cuboidOne = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 1, SideLengthZ = 1 };
            Position positionOne = new Position();
            Cuboid cuboidTwo = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 3, SideLengthZ = 1 };
            Position positionTwo = new Position { X = 0, Y = - 2, Z = 0 };

            ICuboidCollisionTester cuboidCollisionTester = new CuboidCollisionTester();

            Assert.IsTrue(cuboidCollisionTester.CuboidsCollide(cuboidOne, positionOne, cuboidTwo, positionTwo));
        }

        [TestMethod]
        public void CuboidsWithDifferentHeightDoNotCollide()
        {
            Cuboid cuboidOne = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 1, SideLengthZ = 1 };
            Position positionOne = new Position();
            Cuboid cuboidTwo = new Cuboid { Center = new Position(), SideLengthX = 1, SideLengthY = 3, SideLengthZ = 1 };
            Position positionTwo = new Position { X = 0, Y = -5, Z = 0 };

            ICuboidCollisionTester cuboidCollisionTester = new CuboidCollisionTester();

            Assert.IsFalse(cuboidCollisionTester.CuboidsCollide(cuboidOne, positionOne, cuboidTwo, positionTwo));
        }
    }
}
