using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day09;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class Day09Tests
    {
        [TestMethod]
        public void State0()
        {
            TestState(
                step: 0,
                expectedCurrentPosition: 0,
                expectedScore: default,
                0);
        }

        [TestMethod]
        public void State1()
        {
            TestState(
                step: 1,
                expectedCurrentPosition: 1,
                expectedScore: default,
                0, 1);
        }

        [TestMethod]
        public void State2()
        {
            TestState(
                step: 2,
                expectedCurrentPosition: 1,
                expectedScore: default,
                0, 2, 1);
        }

        [TestMethod]
        public void State3()
        {
            TestState(
                step: 3,
                expectedCurrentPosition: 3,
                expectedScore: default,
                0, 2, 1, 3);
        }

        [TestMethod]
        public void State4()
        {
            TestState(
                step: 4,
                expectedCurrentPosition: 1,
                expectedScore: default,
                0, 4, 2, 1, 3);
        }

        [TestMethod]
        public void State5()
        {
            TestState(
                step: 5,
                expectedCurrentPosition: 3,
                expectedScore: default,
                0, 4, 2, 5, 1, 3);
        }

        [TestMethod]
        public void State6()
        {
            TestState(
                step: 6,
                expectedCurrentPosition: 5,
                expectedScore: default,
                0, 4, 2, 5, 1, 6, 3);
        }

        [TestMethod]
        public void State7()
        {
            TestState(
                step: 7,
                expectedCurrentPosition: 7,
                expectedScore: default,
                0, 4, 2, 5, 1, 6, 3, 7);
        }

        [TestMethod]
        public void State8()
        {
            TestState(
                step: 8,
                expectedCurrentPosition: 1,
                expectedScore: default,
                0, 8, 4, 2, 5, 1, 6, 3, 7);
        }

        [TestMethod]
        public void State9()
        {
            TestState(
                step: 9,
                expectedCurrentPosition: 3,
                expectedScore: default,
                0, 8, 4, 9, 2, 5, 1, 6, 3, 7);
        }

        [TestMethod]
        public void State10()
        {
            TestState(
                step: 10,
                expectedCurrentPosition: 5,
                expectedScore: default,
                0, 8, 4, 9, 2, 10, 5, 1, 6, 3, 7);
        }

        [TestMethod]
        public void State11()
        {
            TestState(
                step: 11,
                expectedCurrentPosition: 7,
                expectedScore: default,
                0, 8, 4, 9, 2, 10, 5, 11, 1, 6, 3, 7);
        }

        [TestMethod]
        public void State12()
        {
            TestState(
                step: 12,
                expectedCurrentPosition: 9,
                expectedScore: default,
                0, 8, 4, 9, 2, 10, 5, 11, 1, 12, 6, 3, 7);
        }

        [TestMethod]
        public void State13()
        {
            TestState(
                step: 13,
                expectedCurrentPosition: 11,
                expectedScore: default,
                0, 8, 4, 9, 2, 10, 5, 11, 1, 12, 6, 13, 3, 7);
        }

        [TestMethod]
        public void State14()
        {
            TestState(
                step: 14,
                expectedCurrentPosition: 13,
                expectedScore: default,
                0, 8, 4, 9, 2, 10, 5, 11, 1, 12, 6, 13, 3, 14, 7);
        }

        [TestMethod]
        public void State15()
        {
            TestState(
                step: 15,
                expectedCurrentPosition: 15,
                expectedScore: default,
                0, 8, 4, 9, 2, 10, 5, 11, 1, 12, 6, 13, 3, 14, 7, 15);
        }

        [TestMethod]
        public void State16()
        {
            TestState(
                step: 16,
                expectedCurrentPosition: 1,
                expectedScore: default,
                0, 16, 8, 4, 9, 2, 10, 5, 11, 1, 12, 6, 13, 3, 14, 7, 15);
        }

        [TestMethod]
        public void State22()
        {
            TestState(
                step: 22,
                expectedCurrentPosition: 13,
                expectedScore: default,
                0, 16, 8, 17, 4, 18, 9, 19, 2, 20, 10, 21, 5, 22, 11, 1, 12, 6, 13, 3, 14, 7, 15);
        }

        [TestMethod]
        public void State23()
        {
            TestState(
                step: 23,
                expectedCurrentPosition: 6,
                expectedScore: 32,
                0, 16, 8, 17, 4, 18, 19, 2, 20, 10, 21, 5, 22, 11, 1, 12, 6, 13, 3, 14, 7, 15);
        }

        [TestMethod]
        public void State24()
        {
            TestState(
                step: 24,
                expectedCurrentPosition: 8,
                expectedScore: default,
                0, 16, 8, 17, 4, 18, 19, 2, 24, 20, 10, 21, 5, 22, 11, 1, 12, 6, 13, 3, 14, 7, 15);
        }

        [TestMethod]
        public void State25()
        {
            TestState(
                step: 25,
                expectedCurrentPosition: 10,
                expectedScore: default,
                0, 16, 8, 17, 4, 18, 19, 2, 24, 20, 25, 10, 21, 5, 22, 11, 1, 12, 6, 13, 3, 14, 7, 15);
        }

        [TestMethod]
        public void Part1Example1()
        {
            Assert.AreEqual(32, Program.Part1(25, 9));
        }

        [TestMethod]
        public void Part1Example2()
        {
            Assert.AreEqual(8317, Program.Part1(1618, 10));
        }

        [TestMethod]
        public void Part1Example3()
        {
            Assert.AreEqual(146373, Program.Part1(7999, 13));
        }

        [TestMethod]
        public void Part1Example4()
        {
            Assert.AreEqual(2764, Program.Part1(1104, 17));
        }

        [TestMethod]
        public void Part1Example5()
        {
            Assert.AreEqual(54718, Program.Part1(6111, 21));
        }

        [TestMethod]
        public void Part1Example6()
        {
            Assert.AreEqual(37305, Program.Part1(5807, 30));
        }

        [TestMethod]
        public void IdgPart1()
        {
            Assert.AreEqual(416424, Program.Part1(Program.LastMarbleValue, Program.Players));
        }

        private static void TestState(
            int step,
            int expectedCurrentPosition,
            int? expectedScore,
            params int[] marbleValues)
        {
            (GameState state, int? turnScore) = Program.GenerateSteps().Skip(step).First();
            Assert.AreEqual(expectedCurrentPosition, state.CurrentMarblePosition, "Position");
            Assert.AreEqual(expectedScore, turnScore, "Score");
            for (int i = 0; i < marbleValues.Length && i < state.Marbles.Count; ++i)
            {
                Assert.AreEqual(marbleValues[i], state.Marbles[i], $"Marble value at {i}");
            }
            Assert.AreEqual(marbleValues.Length, state.Marbles.Count, "Marble count");
            Assert.AreEqual(step, state.LastTurn, "Turn");
        }
    }
}
