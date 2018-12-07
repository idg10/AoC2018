using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day05;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class Day05Tests
    {
        [TestMethod]
        public void Example1()
        {
            Assert.IsTrue(Program.ProcessOneReaction("aA", out string result));
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void Example2()
        {
            Assert.IsTrue(Program.ProcessOneReaction("abBA", out string result));
            Assert.AreEqual("aA", result);
        }

        [TestMethod]
        public void Example3()
        {
            Assert.IsFalse(Program.ProcessOneReaction("abAB", out string result));
            Assert.AreEqual("abAB", result);
        }

        [TestMethod]
        public void Example4()
        {
            Assert.IsFalse(Program.ProcessOneReaction("aabAAB", out string result));
            Assert.AreEqual("aabAAB", result);
        }

        [TestMethod]
        public void Example5()
        {
            var steps = Program.ProcessAllReactions("dabAcCaCBAcCcaDA").ToList();
            string[] expectedSteps =
            {
                "dabAcCaCBAcCcaDA",
                "dabAaCBAcCcaDA",
                "dabCBAcCcaDA",
                "dabCBAcaDA"
            };
            Assert.AreEqual(steps.Count, expectedSteps.Length);
            Assert.IsTrue(expectedSteps.SequenceEqual(steps));
        }

        [TestMethod]
        public void Part1ExampleResult()
        {
            Assert.AreEqual(10, Program.Part1("dabAcCaCBAcCcaDA"));
        }
    }
}
