using System;
using System.Collections.Immutable;
using System.Linq;
using Common;
using LanguageExt.Parsec;

using static Common.Parsers;
using static LanguageExt.Parsec.Prim;
namespace Day08
{
    public static class Program
    {
        public const string ExampleInput = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";

        public static readonly Parser<Node> NodeParser = lazyp(() =>
            from childCount in pInt32
            from metadataCount in pInt32
            from children in count(childCount, NodeParser)
            from metadata in count(metadataCount, pInt32)
            select new Node(
                ImmutableList.CreateRange(children),
                ImmutableList.CreateRange(metadata)));

        public static Node ExampleNode = ProcessLine(NodeParser, ExampleInput);

        static void Main(string[] args)
        {
            Node inputNode = InputReader.ParseLines(typeof(Program), NodeParser).Single();
            int part1 = Part1(inputNode);
            Console.WriteLine("Part 1: " + part1);
        }

        public static int Part1(Node node) => EnumerableEx
            .Return(node)
            .Expand(n => n.Children)
            .SelectMany(n => n.Metadata)
            .Sum();
    }
}
