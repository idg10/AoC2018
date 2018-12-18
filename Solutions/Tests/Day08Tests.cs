using Day08;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Day08.Program;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class Day08Tests
    {
        [TestMethod]
        public void CanParseTestInput()
        {
            Node nodeA = ExampleNode;

            Assert.AreEqual(2, nodeA.Children.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 1, 1, 2 }, nodeA.Metadata));

            Node nodeB = nodeA.Children[0];
            Node nodeC = nodeA.Children[1];

            Assert.AreEqual(0, nodeB.Children.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 10, 11, 12 }, nodeB.Metadata));

            Assert.AreEqual(1, nodeC.Children.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 2 }, nodeC.Metadata));

            Node nodeD = nodeC.Children[0];

            Assert.AreEqual(0, nodeD.Children.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 99 }, nodeD.Metadata));
        }

        [TestMethod]
        public void Part1Example()
        {
            Assert.AreEqual(138, Part1(ExampleNode));
        }
    }
}
