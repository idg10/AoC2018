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
    }
}
