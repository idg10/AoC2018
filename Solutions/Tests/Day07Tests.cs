using Common;
using Day07;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Day07Tests
    {
        [TestMethod]
        public void Part1Example()
        {
            string sequence = Program.Part1(Program.ExampleInput);
            Assert.AreEqual("CABDFE", sequence);
        }

        [TestMethod]
        public void Part1IdgSolution()
        {
            string part1 = Program.Part1(InputReader.ParseLines(typeof(Program), Program.LineParser));
            Assert.AreEqual("BCADPVTJFZNRWXHEKSQLUYGMIO", part1);
        }
    }
}
