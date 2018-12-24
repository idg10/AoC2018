using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Day10;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Day10Tests
    {
        [TestMethod]
        public void PointMoveExample()
        {
            var pt0 = new Point(3, 9, 1, -2);
            var pt1 = pt0.Next();
            var pt2 = pt1.Next();
            var pt3 = pt2.Next();
            Assert.AreEqual(6, pt3.X, "X");
            Assert.AreEqual(3, pt3.Y, "Y");
        }

        [TestMethod]
        public void ExampleDivergenceTest()
        {
            int divergesOn = Program.RunUntilDivergent(Program.ExamplePoints).Take(100).Count();
            Assert.AreEqual(5, divergesOn);
        }
    }
}
