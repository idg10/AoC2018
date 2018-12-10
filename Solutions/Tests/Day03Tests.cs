using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Day03;
using System.Linq;
using System.Collections.Immutable;

namespace Tests
{
    [TestClass]
    public class Day03Tests
    {
        [TestMethod]
        public void ParseExampleLine1()
        {
            Assert.AreEqual(
                (1, 1, 3, 4, 4),
                Program.ExampleLines[0]);
        }

        [TestMethod]
        public void ParseExampleLine2()
        {
            Assert.AreEqual(
                (2, 3, 1, 4, 4),
                Program.ExampleLines[1]);
        }

        [TestMethod]
        public void ParseExampleLine3()
        {
            Assert.AreEqual(
                (3, 5, 5, 2, 2),
                Program.ExampleLines[2]);
        }

        [TestMethod]
        public void CountOverlappingCellsInExample()
        {
            IImmutableDictionary<(int x, int y), IImmutableSet<int>> state = BuildStateFromExample();
            Assert.AreEqual(4, Program.CountOverlappingCells(state));
        }

        [TestMethod]
        public void FindNonOverlappingClaimInExample()
        {
            IImmutableDictionary<(int x, int y), IImmutableSet<int>> state = BuildStateFromExample();
            Assert.AreEqual(3, Program.FindNonOverlappingClaim(state));
        }

        private static IImmutableDictionary<(int x, int y), IImmutableSet<int>> BuildStateFromExample()
            => Program.ExampleLines
                .Aggregate(Program.GetEmptyState<int>(), Program.AddRectangle);
    }
}
