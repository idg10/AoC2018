using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day11;

namespace Tests
{
    [TestClass]
    public class Day11Tests
    {
        [TestMethod]
        public void PowerLevelExample1()
        {
            Assert.AreEqual(4, Program.CalculatePower(3, 5, 8));
        }

        [TestMethod]
        public void PowerLevelExample2()
        {
            Assert.AreEqual(-5, Program.CalculatePower(122, 79, 57));
        }

        [TestMethod]
        public void PowerLevelExample3()
        {
            Assert.AreEqual(0, Program.CalculatePower(217, 196, 39));
        }

        [TestMethod]
        public void PowerLevelExample4()
        {
            Assert.AreEqual(4, Program.CalculatePower(101, 153, 71));
        }

        [TestMethod]
        public void PowerGridExample1Sums()
        {
            // Verify first row of their example grid matches cell by cell.
            Assert.AreEqual(-2, Program.CalculatePower(32, 44, 18));
            Assert.AreEqual(-4, Program.CalculatePower(33, 44, 18));
            Assert.AreEqual(4, Program.CalculatePower(34, 44, 18));
            Assert.AreEqual(4, Program.CalculatePower(35, 44, 18));
            Assert.AreEqual(4, Program.CalculatePower(36, 44, 18));

            // Verify second row of their example grid matches cell by cell.
            Assert.AreEqual(-4, Program.CalculatePower(32, 45, 18));
            Assert.AreEqual(4, Program.CalculatePower(33, 45, 18));
            Assert.AreEqual(4, Program.CalculatePower(34, 45, 18));
            Assert.AreEqual(4, Program.CalculatePower(35, 45, 18));
            Assert.AreEqual(-5, Program.CalculatePower(36, 45, 18));

            // Verify that our windowed sums for the first row look correct.
            // -2  -4   4   4   4
            // ----------           : -2  + -4 + 4 = -2
            //     ----------       : -4  +  4 + 4 =  4
            //          ---------   :  4  +  4 + 4 = 12
            int[] row1 = Program.GenerateSummedRow(3, 44, 18);
            Assert.AreEqual(-2, row1[32]);
            Assert.AreEqual(4, row1[33]);
            Assert.AreEqual(12, row1[34]);

            // Verify that our 2D windowed sums over the 1st 3 rows look correct.
            int[,] grid = Program.GenerateSummedGrid(3, 3, 18);
            Assert.AreEqual(12, grid[32, 44]);
            Assert.AreEqual(26, grid[33, 44]);
            Assert.AreEqual(18, grid[34, 44]);

            // ...and finally their actual example
            Assert.AreEqual(29, grid[33, 45]);
        }

        [TestMethod]
        public void PowerGridExample2Sum()
        {
            Assert.AreEqual(30, Program.GenerateSummedGrid(3, 3, 42)[21, 61]);
        }

        [TestMethod]
        public void Part1Example1()
        {
            Assert.AreEqual((x: 33, y: 45), Program.SolvePart1(18));
        }

        [TestMethod]
        public void Part1Example2()
        {
            Assert.AreEqual((x: 21, y: 61), Program.SolvePart1(42));
        }

        [TestMethod]
        public void Part2Example1()
        {
            Assert.AreEqual((x: 90, y: 269, size: 16), Program.SolvePart2(18));
        }

        [TestMethod]
        public void Part2Example2()
        {
            Assert.AreEqual((x: 232, y: 251, size: 12), Program.SolvePart2(42));
        }
    }
}
