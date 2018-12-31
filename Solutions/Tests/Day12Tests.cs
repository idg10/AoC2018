using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Day12;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Day12Tests
    {
        private static readonly string[] ExpectedTestResults =
        {
            "...#..#.#..##......###...###...........",
            "...#...#....#.....#..#..#..#...........",
            "...##..##...##....#..#..#..##..........",
            "..#.#...#..#.#....#..#..#...#..........",
            "...#.#..#...#.#...#..#..##..##.........",
            "....#...##...#.#..#..#...#...#.........",
            "....##.#.#....#...#..##..##..##........",
            "...#..###.#...##..#...#...#...#........",
            "...#....##.#.#.#..##..##..##..##.......",
            "...##..#..#####....#...#...#...#.......",
            "..#.#..#...#.##....##..##..##..##......",
            "...#...##...#.#...#.#...#...#...#......",
            "...##.#.#....#.#...#.#..##..##..##.....",
            "..#..###.#....#.#...#....#...#...#.....",
            "..#....##.#....#.#..##...##..##..##....",
            "..##..#..#.#....#....#..#.#...#...#....",
            ".#.#..#...#.#...##...#...#.#..##..##...",
            "..#...##...#.#.#.#...##...#....#...#...",
            "..##.#.#....#####.#.#.#...##...##..##..",
            ".#..###.#..#.#.#######.#.#.#..#.#...#..",
            ".#....##....#####...#######....#.#..##.",
        };

        [TestMethod]
        public void TestInputProducesExpectedResults()
        {
            Assert.AreEqual(ExpectedTestResults.Length, Program.TestResults.Length, "Count");
            for (int i = 0; i < ExpectedTestResults.Length; ++i)
            {
                string expectedPots = ExpectedTestResults[i];
                int leftPad = expectedPots.TakeWhile(c => c == '.').Count();
                int expectedOffset = leftPad - 3;   // Example data starts at offset -3
                string trimmedExpectedPots = expectedPots.Substring(leftPad).TrimEnd('.');

                Assert.AreEqual(trimmedExpectedPots, Program.TestResults[i].PotText, "Row " + i);
                Assert.AreEqual(expectedOffset, Program.TestResults[i].LeftIndex, "LeftIndex " + i);
            }
        }

        [TestMethod]
        public void Part1Example()
        {
            Assert.AreEqual(
                325,
                Program.SolvePart1(Program.TestInitialState, Program.TestRulesProducingPots, 20));
        }
    }
}
