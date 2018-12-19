using Day08;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Day08.Program;
using System.Linq;
using System.Collections.Immutable;
using Common;
using System.IO;

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

        [TestMethod]
        public void WhenNodeHasNoChildrenAndNoMetadataValueIsZero()
        {
            var n = new Node(ImmutableList<Node>.Empty, ImmutableList<int>.Empty);
            Assert.AreEqual(0, n.CalculateValue());
        }

        [TestMethod]
        public void WhenNodeHasNoChildrenAndSomeMetadataValueIsSumOfMetadata()
        {
            var n = new Node(
                ImmutableList<Node>.Empty,
                ImmutableList.Create(1, 2, 3, 4));
            Assert.AreEqual(10, n.CalculateValue());
        }

        [TestMethod]
        public void WhenNodeHasChildrenAndNoMetadataValueIsZero()
        {
            var cn1 = new Node(
                ImmutableList<Node>.Empty,
                ImmutableList.Create(1, 2, 3, 4));
            var cn2 = new Node(
                ImmutableList<Node>.Empty,
                ImmutableList.Create(19, 23));
            var n = new Node(ImmutableList.Create(cn1, cn2), ImmutableList<int>.Empty);
            Assert.AreEqual(0, n.CalculateValue());
        }

        [TestMethod]
        public void WhenNodeHasChildrenAndMetadataThatIsAllOutOfRangeValueIsZero()
        {
            var cn1 = new Node(
                ImmutableList<Node>.Empty,
                ImmutableList.Create(1, 2, 3, 4));
            var cn2 = new Node(
                ImmutableList<Node>.Empty,
                ImmutableList.Create(19, 23));
            var n = new Node(ImmutableList.Create(cn1, cn2), ImmutableList.Create(99, 1234));
            Assert.AreEqual(0, n.CalculateValue());
        }

        [TestMethod]
        public void WhenNodeHasChildrenAndMetadataReferringToFirstChildValueIsChildValue()
        {
            var cn1 = new Node(
                ImmutableList<Node>.Empty,
                ImmutableList.Create(1, 2, 3, 4));
            var cn2 = new Node(
                ImmutableList<Node>.Empty,
                ImmutableList.Create(19, 23));
            var n = new Node(ImmutableList.Create(cn1, cn2), ImmutableList.Create(1));
            Assert.AreEqual(10, n.CalculateValue());
        }

        [TestMethod]
        public void WhenNodeHasChildrenAndMetadataReferringToSecondChildValueIsChildValue()
        {
            var cn1 = new Node(
                ImmutableList<Node>.Empty,
                ImmutableList.Create(1, 2, 3, 4));
            var cn2 = new Node(
                ImmutableList<Node>.Empty,
                ImmutableList.Create(19, 23));
            var n = new Node(ImmutableList.Create(cn1, cn2), ImmutableList.Create(2));
            Assert.AreEqual(42, n.CalculateValue());
        }

        [TestMethod]
        public void Part2Example()
        {
            Assert.AreEqual(66, ExampleNode.CalculateValue());
        }

        [TestMethod]
        public void IdgPart1()
        {
            if (!LiveTestHacks.RunningInVsLiveTest)
            {
                Node inputNode = InputReader.ParseLines(typeof(Program), NodeParser).Single();
                Assert.AreEqual(41926, Part1(inputNode));

            }
        }

        [TestMethod]
        public void IdgPart2()
        {
            if (!LiveTestHacks.RunningInVsLiveTest)
            {
                Node inputNode = InputReader.ParseLines(typeof(Program), NodeParser).Single();
                Assert.AreEqual(24262, inputNode.CalculateValue());
            }
        }
    }
}
