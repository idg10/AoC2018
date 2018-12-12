using System.Linq;
using Day06;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Day06Tests
    {
        [TestMethod]
        public void UpperQuadrant()
        {
            Assert.AreEqual(Quadrants.Top, Program.GetQuadrants((5, 5), (1, 0)));
            Assert.AreEqual(Quadrants.Top, Program.GetQuadrants((5, 5), (5, 0)));
            Assert.AreEqual(Quadrants.Top, Program.GetQuadrants((5, 5), (4, 3)));
            Assert.AreEqual(Quadrants.Top, Program.GetQuadrants((5, 5), (5, 3)));
            Assert.AreEqual(Quadrants.Top, Program.GetQuadrants((5, 5), (5, 3)));
            Assert.AreEqual(Quadrants.Top, Program.GetQuadrants((5, 5), (6, 3)));
            Assert.AreEqual(Quadrants.Top, Program.GetQuadrants((5, 5), (5, 4)));
        }

        [TestMethod]
        public void UpperAndRightQuadrants()
        {
            Assert.AreEqual(Quadrants.Top | Quadrants.Right, Program.GetQuadrants((5, 5), (6, 4)));
            Assert.AreEqual(Quadrants.Top | Quadrants.Right, Program.GetQuadrants((5, 5), (10, 0)));
        }

        [TestMethod]
        public void RightQuadrant()
        {
            Assert.AreEqual(Quadrants.Right, Program.GetQuadrants((5, 5), (10, 1)));
            Assert.AreEqual(Quadrants.Right, Program.GetQuadrants((5, 5), (10, 5)));
            Assert.AreEqual(Quadrants.Right, Program.GetQuadrants((5, 5), (7, 4)));
            Assert.AreEqual(Quadrants.Right, Program.GetQuadrants((5, 5), (7, 5)));
            Assert.AreEqual(Quadrants.Right, Program.GetQuadrants((5, 5), (7, 5)));
            Assert.AreEqual(Quadrants.Right, Program.GetQuadrants((5, 5), (7, 6)));
            Assert.AreEqual(Quadrants.Right, Program.GetQuadrants((5, 5), (6, 5)));
        }

        [TestMethod]
        public void BottomAndRightQuadrants()
        {
            Assert.AreEqual(Quadrants.Bottom | Quadrants.Right, Program.GetQuadrants((5, 5), (6, 6)));
            Assert.AreEqual(Quadrants.Bottom | Quadrants.Right, Program.GetQuadrants((5, 5), (10, 10)));
        }

        [TestMethod]
        public void BottomQuadrant()
        {
            Assert.AreEqual(Quadrants.Bottom, Program.GetQuadrants((5, 5), (1, 10)));
            Assert.AreEqual(Quadrants.Bottom, Program.GetQuadrants((5, 5), (5, 10)));
            Assert.AreEqual(Quadrants.Bottom, Program.GetQuadrants((5, 5), (4, 7)));
            Assert.AreEqual(Quadrants.Bottom, Program.GetQuadrants((5, 5), (5, 7)));
            Assert.AreEqual(Quadrants.Bottom, Program.GetQuadrants((5, 5), (5, 7)));
            Assert.AreEqual(Quadrants.Bottom, Program.GetQuadrants((5, 5), (6, 7)));
            Assert.AreEqual(Quadrants.Bottom, Program.GetQuadrants((5, 5), (5, 6)));
        }

        [TestMethod]
        public void BottomAndLeftQuadrants()
        {
            Assert.AreEqual(Quadrants.Bottom | Quadrants.Left, Program.GetQuadrants((5, 5), (4, 6)));
            Assert.AreEqual(Quadrants.Bottom | Quadrants.Left, Program.GetQuadrants((5, 5), (0, 10)));
        }

        [TestMethod]
        public void LeftQuadrant()
        {
            Assert.AreEqual(Quadrants.Left, Program.GetQuadrants((5, 5), (0, 1)));
            Assert.AreEqual(Quadrants.Left, Program.GetQuadrants((5, 5), (0, 5)));
            Assert.AreEqual(Quadrants.Left, Program.GetQuadrants((5, 5), (3, 4)));
            Assert.AreEqual(Quadrants.Left, Program.GetQuadrants((5, 5), (3, 5)));
            Assert.AreEqual(Quadrants.Left, Program.GetQuadrants((5, 5), (3, 5)));
            Assert.AreEqual(Quadrants.Left, Program.GetQuadrants((5, 5), (3, 6)));
            Assert.AreEqual(Quadrants.Left, Program.GetQuadrants((5, 5), (4, 5)));
        }

        [TestMethod]
        public void UpperAndLeftQuadrants()
        {
            Assert.AreEqual(Quadrants.Top | Quadrants.Left, Program.GetQuadrants((5, 5), (4, 4)));
            Assert.AreEqual(Quadrants.Top | Quadrants.Left, Program.GetQuadrants((5, 5), (0, 0)));
        }

        [TestMethod]
        public void AllQuadrants()
        {
            // Not strictly reqiured by the challenge, but the tests feel incomplete without this.
            Assert.AreEqual(
                Quadrants.Top | Quadrants.Left | Quadrants.Right | Quadrants.Bottom,
                Program.GetQuadrants((5, 5), (5, 5)));
        }

        [TestMethod]
        public void ExampleFiniteRegions()
        {
            var regions = Program.GetFiniteRegionCentres(Program.ExampleInput);
            Assert.AreEqual(2, regions.Count());
            Assert.IsTrue(regions.Contains((3,4)), "(3,4)");
            Assert.IsTrue(regions.Contains((5,5)), "(5,5)");
        }

        [TestMethod]
        public void ExampleBoundingBox()
        {
            var size = Program.GetBoundingBoxFromOrigin(Program.ExampleInput);
            Assert.AreEqual((8, 9), size);
        }

        [TestMethod]
        public void ExampleFiniteRegionSizes()
        {
            var exampleMap = Program.PopulateRegions(10, 10, Program.ExampleInput);
            var regionSizes = Program.GetFiniteRegionSizes(exampleMap, Program.ExampleInput);
            Assert.AreEqual(2, regionSizes.Count);
            Assert.AreEqual(9, regionSizes[(3, 4)]);
            Assert.AreEqual(17, regionSizes[(5, 5)]);
        }

        [TestMethod]
        public void Part1Example()
        {
            int result = Program.SolvePart1(Program.ExampleInput);
            Assert.AreEqual(17, result);
        }
    }
}
